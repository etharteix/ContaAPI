using System;
using System.Collections.Generic;
using ContaAPI.Domain.Models;

namespace ContaAPI.Domain.Interfaces
{
    public interface IServiceUser
    {
        UserModel Insert(CreateUserModel userModel);

        UserModel Update(Guid id, UpdateUserModel userModel);

        void Delete(Guid id);

        UserModel RecoverById(Guid id);

        IEnumerable<UserModel> RecoverAll();
    }
}
