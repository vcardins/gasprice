/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using GasPrice.Core.Models;

namespace GasPrice.Core.Account
{
    public class UserKey : BaseObjectState
    {
        public virtual int Id { get; set; }
        public virtual int UserId { get; set; }
    }
}
