using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TechsApp.Infrastructure.Storage.Providers
{
    public class FileSystemStorageProvider : StorageProvider
    {
        private string _storageRoot;
        private JavaScriptSerializer _js;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            _storageRoot = config.GetAndRemove<string>("storageRoot", true);
            if (!string.IsNullOrEmpty(_storageRoot))
            {
                _storageRoot = HttpContext.Current.Server.MapPath(_storageRoot);
            }

            _js = new JavaScriptSerializer { MaxJsonLength = 41943040 };
        }

        public override void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> status)
        {
            if (context.Request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var inputStream = context.Request.Files[0].InputStream;
            var fullName = _storageRoot + Path.GetFileName(fileName);

            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }
                fs.Flush();
                fs.Close();
            }
            status.Add(new FilesStatus(new FileInfo(fullName)));
        }

        public override FilesStatus UploadSingleFile(HttpPostedFile file)
        {
            string fullName = Path.GetFileName(file.FileName);
            if (fullName != null)
            {
                var path = Path.Combine(_storageRoot, fullName);

                file.SaveAs(path);
            
                return new FilesStatus(fullName, file.ContentLength, path);
            }
            throw new Exception();
        }

        public void UploadWholeFileEx(HttpFileCollection files, List<FilesStatus> status)
        {
            for (var i = 0; i< files.Count; i++)
            {
                HttpPostedFile file = files[i];

                status.Add(UploadSingleFile(file));
            }
        }

        public override void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            UploadWholeFileEx(context.Request.Files, statuses);
        }

        public override void UploadFile(HttpContext context)
        {
            var statuses = new List<FilesStatus>();
            var headers = context.Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(context, statuses);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], context, statuses);
            }

            WriteJsonIframeSafe(context, statuses);
        }

        public override void DeliverFile(HttpContext context)
        {
            var filename = context.Request["f"];
            var filePath = _storageRoot + filename;

            if (File.Exists(filePath))
            {
                context.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + filename + "\"");
                context.Response.ContentType = "application/octet-stream";
                context.Response.ClearContent();
                context.Response.WriteFile(filePath);
            }
            else
                context.Response.StatusCode = 404;
        }

        public override void DeleteFile(HttpContext context)
        {
            var filePath = _storageRoot + context.Request["f"];
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        private void WriteJsonIframeSafe(HttpContext context, List<FilesStatus> statuses)
        {
            context.Response.AddHeader("Vary", "Accept");
            try
            {
                context.Response.ContentType = context.Request["HTTP_ACCEPT"].Contains("application/json") ? "application/json" : "text/plain";
            }
            catch
            {
                context.Response.ContentType = "text/plain";
            }

            var jsonObj = _js.Serialize(statuses.ToArray());
            context.Response.Write(jsonObj);
        }

        public override void ListCurrentFiles(HttpContext context)
        {
            var files =
                new DirectoryInfo(_storageRoot)
                    .GetFiles("*", SearchOption.TopDirectoryOnly)
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Select(f => new FilesStatus(f))
                    .ToArray();

            string jsonObj = _js.Serialize(files);
            context.Response.AddHeader("Content-Disposition", "inline; filename=\"files.json\"");
            context.Response.Write(jsonObj);
            context.Response.ContentType = "application/json";
        }
    }
}