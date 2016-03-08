using System.Collections.Generic;
using System.Web;
using TechsApp.Core.Extensions;

namespace TechsApp.Infrastructure.Storage.Providers
{
    /// <summary>
    ///  all functionality that is specific to azure
    /// </summary>
    public class AzureStorageProvider : StorageProvider
    {
        private string _storageKey;

        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);

            _storageKey = config.GetAndRemove<string>("storageKey", true);
        }

        public override void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> status)
        {
            throw new System.NotImplementedException();
        }

        public override void UploadWholeFile(HttpContext context, List<FilesStatus> statuses)
        {
            throw new System.NotImplementedException();
        }

        public override void UploadFile(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void DeliverFile(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void DeleteFile(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public override void ListCurrentFiles(HttpContext context)
        {
            throw new System.NotImplementedException();
        }

        public override FilesStatus UploadSingleFile(HttpPostedFile file)
        {
            throw new System.NotImplementedException();
        }
    }
}