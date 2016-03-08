/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Security.Claims;
using GasPrice.Core.EventHandling;

namespace GasPrice.Core.Account.Events
{
    public class GetTwoFactorAuthToken : ICommand
    {
        public UserAccount Account { get; set; }
        public string Token { get; set; }
    }

    public class IssueTwoFactorAuthToken : ICommand
    {
        public UserAccount Account { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
    }

    public class ClearTwoFactorAuthToken : ICommand
    {
        public UserAccount Account { get; set; }
    }
    
    public class GetValidationMessage : ICommand
    {
        public string ID { get; set; }
        public string Message { get; set; }
    }

    public class MapClaimsFromAccount : ICommand
    {
        public UserAccount Account { get; set; }
        public IEnumerable<Claim> MappedClaims { get; set; }
    }

    public class MapClaimsToAccount : ICommand
    {
        public UserAccount Account { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
}
