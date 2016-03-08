/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;

namespace GasPrice.Core.Account.Query
{
    public class GroupQueryResult
    {
        public Guid ID { get; set; }
        public string Tenant { get; set; }
        public string Name { get; set; }
    }
}
