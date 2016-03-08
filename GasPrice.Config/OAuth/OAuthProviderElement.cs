using System.Configuration;
using GasPrice.Core.Common.Enums;
using GasPrice.Core.Config.OAuth;

namespace GasPrice.Config.OAuth
{
    public class OAuthProviderElement : ConfigurationElement, IOAuthProviderSettings
    {
        [ConfigurationProperty("name")]
        public OAuthProvider Name
        {
            get { return (OAuthProvider)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("secret")]
        public string Secret
        {
            get { return (string)this["secret"]; }
            set { this["secret"] = value; }
        }

        [ConfigurationProperty("verifyTokenEndPoint")]
        public string VerifyTokenEndPoint
        {
            get { return (string)this["verifyTokenEndPoint"]; }
            set { this["verifyTokenEndPoint"] = value; }
        }
    }

}
