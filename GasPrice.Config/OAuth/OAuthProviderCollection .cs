using System.Configuration;
using GasPrice.Core.Common.Enums;

namespace GasPrice.Config.OAuth
{
    /// <summary>
    /// The photo resize collection.
    /// </summary>
    [ConfigurationCollection(typeof(OAuthProviderElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]

    public class OAuthProviderCollection : BaseElementCollection<OAuthProviderElement, OAuthProvider>
    {
     
    }
}