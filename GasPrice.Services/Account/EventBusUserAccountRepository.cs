using System;
using System.Linq;
using System.Linq.Expressions;
using GasPrice.Core.Account;
using GasPrice.Core.Account.Repository;
using GasPrice.Core.Data.Infrastructure;
using GasPrice.Core.Data.Repositories;
using GasPrice.Core.Data.UnitOfWork;
using GasPrice.Core.EventHandling;

namespace GasPrice.Services.Account
{
    public class EventBusUserAccountRepository : IUserAccountRepository
    {
        readonly IEventSource _source;
        internal IRepositoryAsync<UserAccount> Inner;
        internal IUnitOfWorkAsync UnitOfWork;
        readonly IEventBus _validationBus;
        readonly IEventBus _eventBus;

        public EventBusUserAccountRepository(IEventSource source, IUnitOfWorkAsync unitOfWork, IEventBus validationBus, IEventBus eventBus)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (validationBus == null) throw new ArgumentNullException("validationBus");
            if (eventBus == null) throw new ArgumentNullException("eventBus");

            _source = source;
            Inner = unitOfWork.RepositoryAsync<UserAccount>();
            UnitOfWork = unitOfWork;
            _validationBus = validationBus;
            _eventBus = eventBus;
        }

        private void RaiseValidation()
        {
            foreach (var evt in _source.GetEvents())
            {
                _validationBus.RaiseEvent(evt);
            }
        }

        private void RaiseEvents()
        {
            foreach (var evt in _source.GetEvents())
            {
                _eventBus.RaiseEvent(evt);
            }

            _source.Clear();
        }

        public UserAccount Create()
        {
            return Inner.Create();
        }

        public void Add(UserAccount item)
        {
            RaiseValidation();
            Inner.InsertOrUpdateGraph(item, true);
            RaiseEvents();
        }

        public void Remove(UserAccount item)
        {
            RaiseValidation();
            Inner.Delete(item, true);
            RaiseEvents();
        }

        public void Update(UserAccount item)
        {
            RaiseValidation();
            item.ObjectState = ObjectState.Modified;
            Inner.Update(item, true);
            RaiseEvents();
        }

        public UserAccount GetByID(Guid id)
        {
            return GetAccount(x => x.ID == id);
        }

        private UserAccount GetAccount(Expression<Func<UserAccount, bool>> query)
        {
            return Inner
                   .Query(query)
                   .Include(u => u.ClaimCollection)
                   .Select(x => x)
                   .FirstOrDefault();
        }

        public UserAccount GetById(int id)
        {
            return GetAccount(x => x.UserId == id);
        }

        public UserAccount GetByUsername(string username)
        {
            return Inner.Query(x => x.Username == username).Select().FirstOrDefault();
        }

        public UserAccount GetByUsername(string tenant, string username)
        {
            return GetAccount(x => x.Tenant == tenant && x.Username == username);
        }

        public UserAccount GetByEmail(string tenant, string email)
        {
            return GetAccount(x => x.Tenant == tenant && x.Email == email);
        }

        public UserAccount GetByVerificationKey(string key)
        {
            return GetAccount(x => x.VerificationKey == key);
        }

    }
}