using System.Configuration;
using App.Core.Enums;

namespace App.Infrastructure.Config.OAuth
{
    /// <summary>
    /// The OAuth providers collection.
    /// </summary>
    [ConfigurationCollection(typeof(OAuthProviderElement), AddItemName = CONST_ELEMENT_NAME,
    CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]

    //[ConfigurationCollection(typeof(OAuthProviderElement), AddItemName = CONST_ELEMENT_NAME, CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class OAuthProviderCollection2 : BaseConfigurationElementCollection<OAuthProviderElement, OAuthProvider>
    {
        #region Constants
        private const string CONST_ELEMENT_NAME = "OAuth";
        #endregion
        /// <summary>
        /// The property name.
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get { return CONST_ELEMENT_NAME; }
        }
    }
}