#region credits
// ***********************************************************************
// Assembly	: GasPrice.Infrastructure
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-21-2013
// ***********************************************************************
#endregion

using System.Configuration;
using GasPrice.Config.Messaging;

namespace GasPrice.Config
{
    #region

    

    #endregion

    public partial class AppConfig
    {
        [ConfigurationProperty("mailer", IsRequired = true)]
        public MailerElement Mailer
        {
            get
            {
                return (MailerElement)base["mailer"];
            }
        }
    }
}
