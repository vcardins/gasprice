#region credits
// ***********************************************************************
// Assembly	: Flext.Core
// Author	: Victor Cardins
// Created	: 03-16-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-21-2013
// ***********************************************************************
#endregion

namespace GasPrice.Core.Config.Messaging
{
    #region

    

    #endregion

    public interface IMailerSettings
    {
        string From { get; set; }
        string Domain { get; set; }
        string Title { get; set; }

    }
}