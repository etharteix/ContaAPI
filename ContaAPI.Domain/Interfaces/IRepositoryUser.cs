using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;

namespace ContaAPI.Domain.Interfaces
{
    public interface IRepositoryUser
    {
        void Save(UserEntity obj);

        void Remove(Guid id);

        UserEntity GetById(Guid id);

        IList<UserEntity> GetAll();

        UserEntity GetByEmail(string email);
    }
}

