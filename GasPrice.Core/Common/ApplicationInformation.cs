
using GasPrice.Core.Common.Information;

namespace GasPrice.Core.Common
{
    public class ApplicationInformation : IApplicationInformation
    {
        public virtual string AppUrl { get; set; }
        public virtual string ApplicationName { get; set; }
        public int NewsletterFrequency { get; set; }
        public string Email { get; set; }
        public string SupportEmail { get; set; }
        public virtual string EmailSignature { get; set; }
        public virtual string LoginUrl { get; set; }
        public virtual string HomePage { get; set; }
        public virtual string ConfirmPasswordResetUrl { get; set; }
        public virtual string ConfirmChangeEmailUrl { get; set; }
        public virtual string CancelVerificationUrl { get; set; }
        public virtual string ServicesAgreementUrl { get; set; }
    }
}
