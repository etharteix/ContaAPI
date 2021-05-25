using System.Collections.Generic;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using ContaAPI.Domain.Entities;
using Flunt.Validations;
using ContaAPI.Infra.Shared.Contexts;
using ContaAPI.Infra.Shared.Mapper;
using System;
using System.Threading.Tasks;
using System.Text;

namespace ContaAPI.Service.Services
{
    public class UserService : IServiceUser
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IRepositoryAccount _repositoryAccount;
        private readonly NotificationContext _notificationContext;

        public UserService(IRepositoryUser repositoryUser, IRepositoryAccount repositoryAccount, NotificationContext notificationContext)
        {
            _repositoryUser = repositoryUser;
            _repositoryAccount = repositoryAccount;
            _notificationContext = notificationContext;
        }

        public IEnumerable<UserModel> RecoverAll()
        {
            var users = _repositoryUser.GetAll();
            return users.ConvertToUsers();
        }

        public UserModel RecoverById(Guid id)
        {
            var user = _repositoryUser.GetById(id);

            if (user == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(user, nameof(user), "User not found."));
                return default;
            }

            return user.ConvertToUser();
        }

        public void Delete(Guid id)
        {
            var user = _repositoryUser.GetById(id);

            if (user == null)
                _notificationContext.AddNotifications(new Contract().IsNotNull(user, nameof(user), "User not found."));

            _repositoryUser.Remove(id);

            DeleteAccount(user.Id);
        }

        public UserModel Insert(CreateUserModel userModel)
        {
            var emailExist = _repositoryUser.GetByEmail(userModel.Email) != null;

            if (emailExist)
            {
                _notificationContext.AddNotifications(new Contract().IsFalse(emailExist, nameof(userModel.Email), "Email already being used."));
                return default;
            }

            var user = userModel.ConvertToUserEntity();
            _notificationContext.AddNotifications(user.Notifications);

            if (_notificationContext.Invalid)
                return default;

            _repositoryUser.Save(user);

            CreateAccount(user.Id);

            return user.ConvertToUser();
        }


        public UserModel Update(Guid id, UpdateUserModel userModel)
        {
            var user = _repositoryUser.GetById(id);

            if (user == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(user, nameof(user), "User not found."));
                return default;
            }

            var emailExist = _repositoryUser.GetByEmail(userModel.Email) != null;

            if (emailExist)
            {
                _notificationContext.AddNotifications(new Contract().IsFalse(emailExist, nameof(userModel.Email), "Email already being used."));
                return default;
            }

            if (userModel.Name == "")
                userModel.Name = user.Name.ToString();

            if (userModel.Email == "")
                userModel.Email = user.Email.ToString();

            UserEntity updatedUser;
            if (userModel.Password == "")
                updatedUser = userModel.ConvertToUserEntity(id);
            else
                updatedUser = userModel.ConvertToUserEntity(id, userModel.Password);

            _notificationContext.AddNotifications(updatedUser.Notifications);

            if (_notificationContext.Invalid)
                return default;

            _repositoryUser.Save(updatedUser);
            return updatedUser.ConvertToUser();
        }

        public UserModel Login(LoginUserModel userModel)
        {
            var user = _repositoryUser.GetByEmail(userModel.Email);

            if (user == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(user, nameof(user), "User not found."));
                return default;
            }

            var correctPwd = Convert.ToBase64String(new UTF8Encoding().GetBytes(userModel.Password)) == user.Password.ToString();

            if (!correctPwd)
            {
                _notificationContext.AddNotifications(new Contract().IsTrue(correctPwd, nameof(userModel.Password), "Wrong password."));
                return default;
            }

            return user.ConvertToUser();
        }

        private void CreateAccount(Guid userId)
        {
            var accountModel = new CreateAccountModel(0, userId);
            _repositoryAccount.Save(accountModel.ConvertToAccountEntity());
        }

        private void DeleteAccount(Guid userId)
        {
            var account = _repositoryAccount.GetByUserId(userId);
            _repositoryAccount.Remove(account.Id);
        }
    }
}
