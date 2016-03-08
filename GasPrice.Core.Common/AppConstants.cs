using System;

namespace GasPrice.Core.Common
{
    public class AppConstants
    {
        public class MessageTemplatesPath
        {
            public const string EmailBody = "GasPrice.Core.Messaging.Templates.Email.{0}.{1}.html";
            public const string EmailSubject = "GasPrice.Core.Messaging.Templates.Email.{0}.Subjects.json";
        }

        public class EmailTemplatesPath
        {
            public static string EmailPath = "GasPrice.Api.Templates.Email";
            public static string EmailBody = String.Format("{0}", EmailPath) + ".{0}.cshtml";
            public static string EmailSubject = String.Format("{0}.{1}", EmailPath, "Subjects.json");
        }
      
    }
}
