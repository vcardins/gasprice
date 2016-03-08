using System.Configuration;
using GasPrice.Config.Images;

namespace GasPrice.Config
{
    public partial class AppConfig
    {
        [ConfigurationProperty("images", IsRequired = true)]
        public ImageConfigurationElement Images
        {
            get
            {
                return (ImageConfigurationElement)base["images"];
            }
        }
    }
}
