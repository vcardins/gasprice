using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Web;

namespace TechsApp.Infrastructure.Storage
{
    // any common functionaltiy, this is a singleton
    public abstract class StorageProvider : ProviderBase
    {
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            // call the code to determine the different modules...
        }


        // figure out what methods need to go here
        // StoreFile(byte[] bytes)
        // GetFile(int id)

        // design goal, we need a clean api here...
        public abstract void UploadPartialFile(string fileName, HttpContext context, List<FilesStatus> status);
        public abstract void UploadWholeFile(HttpContext context, List<FilesStatus> statuses);
        public abstract void UploadFile(HttpContext context);
        public abstract void DeliverFile(HttpContext context);
        public abstract void DeleteFile(HttpContext context);
        public abstract void ListCurrentFiles(HttpContext context);
        public abstract FilesStatus UploadSingleFile(HttpPostedFile file);
    }
}
