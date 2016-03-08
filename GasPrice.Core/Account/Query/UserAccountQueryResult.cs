/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;

namespace GasPrice.Core.Account.Query
{
    public class UserAccountQueryResult
    {
        public int Id { get; set; }
        public Guid ID { get; set; }
        public string Tenant { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
