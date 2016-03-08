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

namespace GasPrice.Core.Config.Messaging
{
    #region

    #endregion
    public interface ISendGridMailerSettings : IMessagingServiceSettings
    {

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>
        /// The SMTP server.
        /// </value>
        string SmtpServer { get; set; }


        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        string Username { get; set; }


        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        string Password { get; set; }


        /// <summary>
        /// Gets or sets from email.
        /// </summary>
        /// <value>
        /// From email.
        /// </value>
        string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets from name.
        /// </summary>
        /// <value>
        /// From name.
        /// </value>
        string FromName { get; set; }

    }
}