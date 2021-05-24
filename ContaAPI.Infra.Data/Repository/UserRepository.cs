using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Infra.Data.Context;

namespace ContaAPI.Infra.Data.Repository
{
    public class UserRepository : BaseRepository<UserEntity, Guid>, IRepositoryUser
    {
        public UserRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
        }

        public void Remove(Guid id) =>
            base.Delete(id);


        public void Save(UserEntity obj)
        {
            if (obj.Id == Guid.Empty)
                base.Insert(obj);
            else
                base.Update(obj, _mySqlContext.Entry(obj).Property(prop => prop.Password));
        }

        public UserEntity GetById(Guid id) =>
            base.Select(id);

        public IList<UserEntity> GetAll() =>
            base.Select();

    }
}
