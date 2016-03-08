using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace GasPrice.Core.Account
{
    public class UserAccountAuthentication
    {
        public Guid ID { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<Claim> Claims { get; set; }
        public AccountManagementStatus? Status { get; set; }
        public UserAccountAuthentication()
        {
            Claims = new List<Claim>();
        }
       
    }
}