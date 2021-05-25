using System;
using System.Collections.Generic;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Infra.Data.Context;
using System.Linq;

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
            {
                if (obj.Password.IsDefined())
                    base.Update(obj);
                else
                    base.Update(obj, _mySqlContext.Entry(obj).Property(prop => prop.Password));
            }
        }

        public UserEntity GetById(Guid id) =>
            base.Select(id);

        public IList<UserEntity> GetAll() =>
            base.Select();

        public UserEntity GetByEmail(string email)
        {
            var users = GetAll();

            foreach (var user in users)
                if (user.Email.ToString().Equals(email))
                    return user;

            return null;
        }

    }
}
