#region credits
// ***********************************************************************
// Assembly	: GasPrice.Core
// Author	: Victor Cardins
// Created	: 03-20-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;
using GasPrice.Core.Account.Enum;
using GasPrice.Core.Models;

namespace GasPrice.Core.Account
{
    public class Audience : Entity
    {
        public int Id { get; set; }
        public Guid ClientId { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public ApplicationTypes ApplicationType { get; set; }
        public bool Active { get; set; }
        public int RefreshTokenLifeTime { get; set; }
        public string AllowedOrigin { get; set; }
        public string AdminEmail { get; set; }
    }
}
