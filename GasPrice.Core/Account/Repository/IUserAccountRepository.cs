/*
 * Copyright (c) Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;

namespace GasPrice.Core.Account.Repository
{
    public interface IUserAccountRepository
    {
        UserAccount Create();
        void Add(UserAccount item);
        void Remove(UserAccount item);
        void Update(UserAccount item); 
        UserAccount GetById(int id);
        UserAccount GetByID(Guid id);
        UserAccount GetByUsername(string username);
        UserAccount GetByUsername(string tenant, string username);
        UserAccount GetByEmail(string tenant, string email);
        UserAccount GetByVerificationKey(string key);
    }

}
