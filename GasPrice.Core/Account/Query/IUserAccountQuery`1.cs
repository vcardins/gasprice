/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace GasPrice.Core.Account.Query
{
    public partial interface IUserAccountQuery
    {
        IEnumerable<UserAccountQueryResult> Query(Func<IQueryable<UserAccount>, IQueryable<UserAccount>> filter);
        IEnumerable<UserAccountQueryResult> Query(Func<IQueryable<UserAccount>, IQueryable<UserAccount>> filter, Func<IQueryable<UserAccount>, IQueryable<UserAccount>> sort, int skip, int count, out int totalCount);
    }
}
