#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

namespace GasPrice.Core.Common.Information
{
    #region

    

    #endregion

    public interface IApplicationInformation
    {
        string AppUrl { get; set; }
        string ApplicationName { get; set; }
        int NewsletterFrequency { get; set; }
        string Email { get; set; }
        string SupportEmail { get; set; }
        string EmailSignature { get; set; }
        string HomePage { get; set; }
        string LoginUrl { get; set; }
        string ConfirmPasswordResetUrl { get; set; }
        string ConfirmChangeEmailUrl { get; set; }
        string CancelVerificationUrl { get; set; }

        //string Description { get; set; }
        //string Copyright { get; set; }
        //string Keywords { get; set; }
    }

}