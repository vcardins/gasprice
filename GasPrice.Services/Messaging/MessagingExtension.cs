using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GasPrice.Core.Common.Messaging.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GasPrice.Services.Messaging
{

    public static class MessagingExtension
    {
        public static List<TCollection> GetJsonTemplate<TCollection>(this string module, string resourcePath)
        {
            var path = String.Format(resourcePath, module);

            var asm = typeof(TCollection).Assembly;
            using (var s = asm.GetManifestResourceStream(path))
            {
                if (s == null) return null;
                using (var sr = new StreamReader(s))
                {
                    var json = sr.ReadToEnd();
                    var obj = (JArray)JsonConvert.DeserializeObject(json);
                    var converted = obj.ToObject<List<TCollection>>();
                    return converted;
                }
            }
        }

        public static TCollection GetTemplate<TCollection>(this string module, string resourcePath, string key) where TCollection : MessageTemplate
        {
            var templates = GetJsonTemplate<TCollection>(module, resourcePath);
            if (templates == null) return null;

            var result = templates.FirstOrDefault(x => x.Key == key);
            return result;
        }

        public static string GetTemplateBody<TCollection>(this string module, string resourcePath, string key) where TCollection : MessageTemplate
        {
            var result = module.GetTemplate<TCollection>(resourcePath, key);
            return result == null ? null : result.Template;
        }

        public static string GetHtmlTemplate<TCollection>(this string module, string resourcePath, string key)
        {
            var path = String.Format(resourcePath, module, key);

            var asm = typeof(TCollection).Assembly;
            using (var s = asm.GetManifestResourceStream(path))
            {
                if (s == null) return null;
                using (var sr = new StreamReader(s))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}

