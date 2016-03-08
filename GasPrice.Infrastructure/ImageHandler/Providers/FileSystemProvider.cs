#region credits
// ***********************************************************************
// Assembly	: GasPrice.Infrastructure
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using GasPrice.Core.Common.Infrastructure.ImageHandler;
using GasPrice.Core.Common.Infrastructure.ImageHandler.Interfaces;

namespace GasPrice.Infrastructure.ImageHandler.Providers
{
    #region

    

    #endregion

    public class FileSystemProvider : ImageProvider
    {
        private string _filePath;

        private const string SearchPattern = ".bmp,.gif,.jpg,.jpeg,.png";

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            _filePath = config["imageFolder"];

            if (string.IsNullOrEmpty(_filePath))
            {
                throw new ProviderException("Empty or missing 'imageFolder' value");
            }

            config.Remove("imageFolder");

            if (config.Count > 0)
            {
                var attr = config.GetKey(0);
                if (!string.IsNullOrWhiteSpace(attr))
                {
                    throw new ProviderException("Unrecognized attribute: " + attr);
                }
            }
        }

        #region Overrides of ImageProvider

        public override Image SaveImageResize(IImageRequest item, string resizeName)
        {
            // todo: if resizeName doesn't exist, then we need to throw an exception
            var photoResize = ImageManager.ImageResizes[resizeName];

            using (var stream = item.Stream)
            {
                string extension = GetExtension(item.MimeType);
                string name = string.Format("{0}{1}", DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_ffff"), extension);
                var photo = SaveImage(
                    stream, photoResize.Width, photoResize.Height, resizeName, name, item.MimeType ?? "image/jpeg");

                return photo;
            }
        }

        public override IList<Image> SaveImageForAllSizes(IImageRequest item, string tag, bool keepOriginalSize)
        {
            var photoResizes = ImageManager.ImageResizes.Where( x => x.Value.Enabled );

            using (var stream = item.Stream)
            {
                string extension = GetExtension(item.MimeType);
                tag = tag ?? DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_ffff");
                string name = string.Format("{0}_{{size}}{1}", tag, extension);

                List<Image> photos =
                    photoResizes.Select(
                        resize =>
                        SaveImage(
                            stream,
                            resize.Value.Width,
                            resize.Value.Height,
                            resize.Key,
                            name.Replace("{size}", string.Format("{0}", resize.Value.Name.ToLower())),
                            item.MimeType)).ToList();

                if (keepOriginalSize)
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
                    {
                        var photo = SaveImage(stream, image.Width, image.Height, null, name, item.MimeType ?? "image/jpeg");
                        photos.Add(photo);
                    }
                }

                return photos;
            }
        }

        public override IList<Image> SaveImageForAllSizes(IImageRequest photoRequest, string container, string photoNamePattern, bool keepOriginalSize)
        {
            throw new NotImplementedException();
        }

        public override Image GetImageResize(string id, ImageSizeTypeEnum sizeName)
        {
            var photo = GetImage(id, sizeName.ToString());

            return photo;
        }

        public override IDictionary<string, Image> GetAllImageResizes(string id)
        {
            var photoResizes = ImageManager.ImageResizes;
            var photos = new Dictionary<string, Image>();

            foreach (var resize in photoResizes)
            {
                var photo = GetImage(id, resize.Key);

                if (photo != null)
                {
                    photos.Add(resize.Key, photo);
                }
            }

            var originalImage = GetImage(id, null);

            if (originalImage != null)
            {
                photos.Add("original", originalImage);
            }

            return photos;
        }

        public override IList<Image> GetImagesByResize(string resizeName, string[] ids)
        {
            string physicalPath = HttpContext.Current.Server.MapPath(_filePath);

            if (!string.IsNullOrWhiteSpace(resizeName))
            {
                physicalPath = Path.Combine(physicalPath, resizeName);
            }

            string resizeFolder = resizeName != null ? string.Format("{0}/", resizeName) : string.Empty;

            // todo: @rsanjar: Add a comment here on what this code is doing.  I think the forula for getting an Id should be in it's own function
            var photos =
                GetFilesByExtensions(physicalPath, SearchPattern.Split(',')).Where(
                    c => ids.Contains(c.Name.Split('.')[0])).Select(
                        c =>
                        new Image
                        {
                            ImageId = c.Name,
                            ResizeName = resizeName,
                            Url = string.Format("{0}/{1}{2}", _filePath.TrimEnd('/'), resizeFolder, c.Name)
                        }).ToList();

            return photos;
        }

        #endregion

        #region private methods

        private Image SaveImage(
            Stream stream,
            int width,
            int height,
            string resizeName,
            string imageFileName,
            string mimeType = "image/jpeg")
        {
            string physicalPath = HttpContext.Current.Server.MapPath(_filePath);

            if (!string.IsNullOrWhiteSpace(resizeName))
            {
                physicalPath = Path.Combine(physicalPath, resizeName);
            }

            // Create a directory for the specified resizeName if not exists
            if (!Directory.Exists(physicalPath))
            {
                Directory.CreateDirectory(physicalPath);
                GrantAccess(physicalPath);
            }

            string fileName = Path.Combine(physicalPath, imageFileName);

            using (var resizedImage = ImageHelper.ResizeImage(stream, width, height))
            {
                var memoryStream = new MemoryStream();
                resizedImage.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                ImageHelper.SaveImage(fileName, resizedImage, 90, mimeType);
            }

            // todo: again, need to isolate the logic to get file path, shouldn't be part of this function
            string extension = GetExtension(mimeType);
            string resizeFolder = resizeName != null ? string.Format("{0}/", resizeName) : string.Empty;

            var photo = new Image
            {
                ImageId = imageFileName.Replace(extension, string.Empty).Trim('.'),
                ResizeName = resizeName,
                Url = string.Format("{0}/{1}{2}", _filePath.TrimEnd('/'), resizeFolder, imageFileName)
            };

            return photo;
        }

        private Image GetImage(string id, string resizeName)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            Image photo = null;
            string physicalPath = System.Web.HttpContext.Current.Server.MapPath(_filePath);

            if (!string.IsNullOrWhiteSpace(resizeName))
            {
                physicalPath = Path.Combine(physicalPath, resizeName);
            }


            if (Directory.Exists(physicalPath))
            {
                var path = Path.Combine(physicalPath, id);

                // todo: need to throw exception if file doesnt exist at all..
                var file = new FileInfo(path);

                // if id doesn't contain extension then search by file name
                if (!file.Exists)
                {
                    file = new DirectoryInfo(physicalPath).GetFiles().FirstOrDefault(c => c.Name.StartsWith(id));
                }

                if (file != null)
                {
                    string resizeFolder = resizeName != null ? string.Format("{0}/", resizeName) : string.Empty;
                    string virtualImagePath = string.Format("{0}/{1}{2}", _filePath.TrimEnd('/'), resizeFolder, file.Name);

                    photo = new Image { ImageId = file.Name.Split('.')[0], ResizeName = resizeName, Url = virtualImagePath };
                }
            }

            return photo;
        }

        private void GrantAccess(string fullPath)
        {
            var directoryInfo = new DirectoryInfo(fullPath);

            var directorySecurity = directoryInfo.GetAccessControl();
            directorySecurity.AddAccessRule(
                new FileSystemAccessRule(
                    "everyone",
                    FileSystemRights.FullControl,
                    InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                    PropagationFlags.NoPropagateInherit,
                    AccessControlType.Allow));
            directoryInfo.SetAccessControl(directorySecurity);
        }

        private IEnumerable<FileInfo> GetFilesByExtensions(string path, params string[] extensions)
        {
            var dirInfo = new DirectoryInfo(path);

            var allowedExtensions = new HashSet<string>(extensions, StringComparer.OrdinalIgnoreCase);

            var photos = dirInfo.GetFiles().Where(f => allowedExtensions.Contains(f.Extension));

            return photos;
        }

        private string GetExtension(string mimeType, string defaultExtension = ".jpeg")
        {
            // todo: need to isolate the logic here for parsing the value
            string extension = mimeType.Split('/').Length > 1 ? mimeType.Split('/')[1] : defaultExtension;
            extension = string.Format(".{0}", extension.Trim('.'));

            if (!SearchPattern.Split(',').Contains(extension))
            {
                extension = ".jpeg";
            }

            return extension;
        }

        #endregion
    }
}