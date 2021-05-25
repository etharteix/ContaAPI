using System.Collections.Generic;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using ContaAPI.Domain.Entities;
using Flunt.Validations;
using ContaAPI.Infra.Shared.Contexts;
using ContaAPI.Infra.Shared.Mapper;
using System;
using System.Text.RegularExpressions;

namespace ContaAPI.Service.Services
{
    public class AccountService : IServiceAccount
    {
        private readonly IRepositoryAccount _repositoryAccount;
        private readonly IRepositoryHistoric _repositoryHistoric;
        private readonly NotificationContext _notificationContext;

        public AccountService(IRepositoryAccount repositoryAccount, IRepositoryHistoric repositoryHistoric, NotificationContext notificationContext)
        {
            _repositoryHistoric = repositoryHistoric;
            _repositoryAccount = repositoryAccount;
            _notificationContext = notificationContext;
        }

        public IEnumerable<AccountModel> RecoverAll()
        {
            var accounts = _repositoryAccount.GetAll();
            return accounts.ConvertToAccounts();
        }

        public AccountModel RecoverById(Guid id)
        {
            var account = _repositoryAccount.GetById(id);

            if (account == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(account, nameof(account), "Account not found."));
                return default;
            }

            return account.ConvertToAccount();
        }

        public AccountModel RecoverByUserId(Guid userId)
        {
            var account = _repositoryAccount.GetByUserId(userId);

            if (account == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(account, nameof(account), "Account not found."));
                return default;
            }

            return account.ConvertToAccount();
        }

        public void Delete(Guid id)
        {
            var account = _repositoryAccount.GetById(id);

            if (account == null)
                _notificationContext.AddNotifications(new Contract().IsNotNull(account, nameof(account), "Account not found."));

            _repositoryAccount.Remove(id);
        }

        public AccountModel Insert(CreateAccountModel accountModel)
        {
            var account = accountModel.ConvertToAccountEntity();
            _notificationContext.AddNotifications(account.Notifications);

            if (_notificationContext.Invalid)
                return default;

            _repositoryAccount.Save(account);
            return account.ConvertToAccount();
        }


        public AccountModel Deposit(Guid userId, UpdateAccountModel accountModel)
        {
            var movement = accountModel.Value;
            var account = UpdateAccount(userId, accountModel, isDeposit: true);
            if (account == null)
                return default;

            _repositoryAccount.Save(account);
            UpdateHistoric("Depósito", movement, userId);

            return account.ConvertToAccount();
        }

        public AccountModel Withdraw(Guid userId, UpdateAccountModel accountModel)
        {
            var movement = accountModel.Value;
            var account = UpdateAccount(userId, accountModel, isDeposit: false);
            if (account == null)
                return default;

            _repositoryAccount.Save(account);
            UpdateHistoric("Retirada", movement, userId);

            return account.ConvertToAccount();
        }

        public AccountModel Payment(Guid userId, PaymentAccountModel paymentAccountModel)
        {
            var updateAccountModel = new UpdateAccountModel(paymentAccountModel.Value);
            var account = UpdateAccount(userId, updateAccountModel, isDeposit: false);
            if (account == null)
                return default;

            var isNullPix = paymentAccountModel.PixCode == null;
            if (isNullPix)
            {
                _notificationContext.AddNotifications(new Contract().IsFalse(isNullPix, nameof(paymentAccountModel.PixCode), "Please inform a pix code."));
                return default;
            }

            var isValidPix = IsValidPix(paymentAccountModel.PixCode);
            if (!isValidPix)
            {
                _notificationContext.AddNotifications(new Contract().IsTrue(isValidPix, nameof(paymentAccountModel.PixCode), "Invalid pix code value."));
                return default;
            }

            _repositoryAccount.Save(account);
            UpdateHistoric("Pagamento", paymentAccountModel.Value, userId);

            return account.ConvertToAccount();
        }

        public void Monetize()
        {
            var accounts = _repositoryAccount.GetAll();

            var cdi = Convert.ToDecimal(Environment.GetEnvironmentVariable("CDI"));
            decimal addBalance;

            foreach (var account in accounts)
            {
                var balance = account.Balance.ToDecimal();
                if (balance > 0)
                {
                    addBalance = decimal.Round(decimal.Multiply(balance, cdi), 2);
                    account.Balance = balance + addBalance;
                    _repositoryAccount.Save(account);
                    UpdateHistoric("Rendimento", addBalance, account.UserId);
                }
            }  
        }

        private AccountEntity UpdateAccount(Guid userId, UpdateAccountModel accountModel, bool isDeposit)
        {
            var account = _repositoryAccount.GetByUserId(userId);
            if (account == null)
            {
                _notificationContext.AddNotifications(new Contract().IsNotNull(account, nameof(account), "Account not found."));
                return null;
            }

            var isNullValue = accountModel.Value == null;
            if (isNullValue)
            {
                _notificationContext.AddNotifications(new Contract().IsFalse(isNullValue, nameof(accountModel.Value), "Please inform a value;"));
                return null;
            }

            var isInvalidValue = accountModel.Value <= 0;
            if (isInvalidValue)
            {
                _notificationContext.AddNotifications(new Contract().IsFalse(isInvalidValue, nameof(accountModel.Value), "Value must be non negative and bigger than zero."));
                return null;
            }

            decimal? totalBalance;
            if (isDeposit)
                totalBalance = account.Balance.ToDecimal() + accountModel.Value;
            else
                totalBalance = account.Balance.ToDecimal() - accountModel.Value;

            accountModel.Value = totalBalance;
            var updatedAccount = accountModel.ConvertToAccountEntity(account.Id, userId);

            _notificationContext.AddNotifications(updatedAccount.Notifications);

            if (_notificationContext.Invalid)
                return null;

            return updatedAccount;
        }

        private bool IsValidPix(string pixCode)
        {
            bool isValid = false;

            //Testa para um email válido
            if (Regex.IsMatch(pixCode, (@"^[a-z0-9.]+@[a-z0-9]+\.+[a-z]+?(\.([a-z]+))?$")))
                isValid = true;

            //Testa para um CPF válido
            if (Regex.IsMatch(pixCode, (@"^([0-9]{3}\.+[0-9]{3}\.[0-9]{3}\-[0-9]{2})|([0-9]{11})$")))
                isValid = true;

            //Testa para um número de telefone válido
            if (Regex.IsMatch(pixCode, (@"^([0-9]{11})|(\([1-9]{2}\)[0-9]{5}\-[0-9]{4})$")))
                isValid = true;

            //Testa para uma chave aleatória válida
            if (Regex.IsMatch(pixCode, (@"^[a-z0-9\-]{32}$")))
                isValid = true;

            return isValid;
        }

        private void UpdateHistoric(string movementType, decimal? movementValue, Guid userId)
        {
            var historicModel = new CreateHistoricModel(movementType, Convert.ToDecimal(movementValue), userId);
            var historic = historicModel.ConvertToHistoricEntity();
            _repositoryHistoric.Save(historic);
        }
    }
}
