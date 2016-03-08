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

namespace GasPrice.Config
{
    #region

    

    #endregion

    public partial class AppConfig : ConfigurationSection
    {
        private static AppConfig _section;

        public static AppConfig Instance
        {
            get
            {
                return (_section ?? (_section = (AppConfig) ConfigurationManager.GetSection("Config")));
            }
        }
    }
}
