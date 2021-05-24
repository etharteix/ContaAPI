using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Infra.Data.Context;
using System.Linq;

namespace ContaAPI.Infra.Data.Repository
{
    public class AccountRepository : BaseRepository<AccountEntity, Guid>, IRepositoryAccount
    {
        public AccountRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
        }

        public void Remove(Guid id) =>
            base.Delete(id);


        public void Save(AccountEntity obj)
        {
            if (obj.Id == Guid.Empty)
                base.Insert(obj);
            else
                base.Update(obj, _mySqlContext.Entry(obj).Property(prop => prop.UserId));
        }

        public AccountEntity GetById(Guid id) =>
            base.Select(id);

        public IList<AccountEntity> GetAll() =>
            base.Select();

        public AccountEntity GetByUserId(Guid userId) =>
            _mySqlContext.Set<AccountEntity>().SingleOrDefault(account => account.UserId == userId);
    }
}
