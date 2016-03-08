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
using GasPrice.Config.Information;

namespace GasPrice.Config
{
    #region

    #endregion

    public partial class AppConfig
    {
        [ConfigurationProperty("information", IsRequired = true)]
        public ApplicationInformationElement Information
        {
            get
            {
                return (ApplicationInformationElement)base["information"];
            }
        }
    }
}
