using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;

namespace ContaAPI.Domain.Interfaces
{
    public interface IRepositoryAccount
    {
        void Save(AccountEntity obj);

        void Remove(Guid id);

        AccountEntity GetById(Guid id);

        IList<AccountEntity> GetAll();

        AccountEntity GetByUserId(Guid id);
    }
}

