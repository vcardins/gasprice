using System.Web;
using TechsApp.Core.Interfaces.Storage;

namespace TechsApp.Infrastructure.Storage.Providers
{
    public class SessionStorageProvider : IStorageProvider
    {
        public T GetValue<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public void SetValue(string key, object value)
        {
            HttpContext.Current.Session[key] = value;
        }
    }
}
