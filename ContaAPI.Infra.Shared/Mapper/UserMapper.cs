using System;
using System.Collections.Generic;
using System.Linq;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Models;

namespace ContaAPI.Infra.Shared.Mapper
{
    public static class UserMapper
    {
        public static UserEntity ConvertToUserEntity(this CreateUserModel userModel) =>
            new UserEntity(Guid.Empty, userModel.Name, userModel.Email, userModel.Password);

        public static UserEntity ConvertToUserEntity(this UpdateUserModel userModel) =>
            new UserEntity(userModel.Id, userModel.Name, userModel.Email);

        public static IEnumerable<UserModel> ConvertToUsers(this IList<UserEntity> users) =>
            new List<UserModel>(users.Select(s => new UserModel(s.Id, s.Name.ToString(), s.Email.ToString())));

        public static UserModel ConvertToUser(this UserEntity user) =>
            new UserModel(user.Id, user.Name.ToString(), user.Email.ToString());
    }
}
