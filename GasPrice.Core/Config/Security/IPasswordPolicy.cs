#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 03-23-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion
namespace GasPrice.Core.Config.Security
{
    public interface IPasswordPolicy
    {
        string PolicyMessage { get; }
        bool ValidatePassword(string password);
    }
}
