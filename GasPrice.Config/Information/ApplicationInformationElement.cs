using System.Configuration;
using System.Web;
using GasPrice.Infrastructure.Extensions;
using GasPrice.Core.Common.Information;

namespace GasPrice.Config.Information
{
    public class ApplicationInformationElement : ConfigurationElement, IApplicationInformation
    {
        [ConfigurationProperty("applicationName")]
        public string ApplicationName
        {
            get { return (string)this["applicationName"]; }
            set { this["applicationName"] = value; }
        }

        public string AppUrl
        {
            get { return HttpContext.Current.GetApplicationUrl(); }
            set { this["appUrl"] = HttpContext.Current.GetApplicationUrl(); } 
        }

        [ConfigurationProperty("emailSignature")]
        public string EmailSignature
        {
            get { return (string)this["emailSignature"]; }
            set { this["emailSignature"] = value; }
        }

        [ConfigurationProperty("homePage")]
        public string HomePage
        {
            get { return (string)this["homePage"]; }
            set { this["homePage"] = value; }
        }

        [ConfigurationProperty("newsletterFrequency")]
        public int NewsletterFrequency
        {
            get { return (int)this["newsletterFrequency"]; }
            set { this["newsletterFrequency"] = value; }
        }

        [ConfigurationProperty("email")]
        public string Email
        {
            get { return (string)this["email"]; }
            set { this["email"] = value; }
        }

        [ConfigurationProperty("supportEmail")]
        public string SupportEmail
        {
            get { return (string)this["supportEmail"]; }
            set { this["supportEmail"] = value; }
        }

        [ConfigurationProperty("loginUrl")]
        public string LoginUrl
        {
            get { return (string)this["loginUrl"]; }
            set { this["loginUrl"] = value; }
        }

        [ConfigurationProperty("confirmChangeEmailUrl")]
        public string ConfirmChangeEmailUrl
        {
            get { return (string)this["confirmChangeEmailUrl"]; }
            set { this["confirmChangeEmailUrl"] = value; }
        }

        [ConfigurationProperty("cancelVerificationUrl")]
        public string CancelVerificationUrl
        {
            get { return (string)this["cancelVerificationUrl"]; }
            set { this["cancelVerificationUrl"] = value; }
        }

        [ConfigurationProperty("confirmPasswordResetUrl")]
        public string ConfirmPasswordResetUrl
        {
            get { return (string)this["confirmPasswordResetUrl"]; }
            set { this["confirmPasswordResetUrl"] = value; }
        }
       
    }

}
