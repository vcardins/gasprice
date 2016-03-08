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
    public interface IMessagingServiceSettings : IBaseElementSettings<string>
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        string Title { get; set; }

    }
}