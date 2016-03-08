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
using GasPrice.Core.Config.Messaging;

namespace GasPrice.Config.Messaging
{

    public class MailerElement : ConfigurationElement, IMailerSettings
    {

        [ConfigurationProperty("from", IsRequired = true)]
        public string From
        {
            get { return (string)base["from"]; }
            set { base["from"] = value; }
        }

        [ConfigurationProperty("domain")]
        public string Domain
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
        }

        [ConfigurationProperty("title")]
        public string Title
        {
            get { return (string)base["title"]; }
            set { base["title"] = value; }
        }
    }
}