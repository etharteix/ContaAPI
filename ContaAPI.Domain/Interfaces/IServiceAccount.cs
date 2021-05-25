using System;
using System.Collections.Generic;
using ContaAPI.Domain.Models;

namespace ContaAPI.Domain.Interfaces
{
    public interface IServiceAccount
    {
        AccountModel Insert(CreateAccountModel accountModel);

        AccountModel Deposit(Guid id, UpdateAccountModel accountModel);

        AccountModel Withdraw(Guid id, UpdateAccountModel accountModel);

        AccountModel Payment(Guid id, PaymentAccountModel paymentAccountModel);

        void Monetize();

        void Delete(Guid id);

        AccountModel RecoverById(Guid id);

        AccountModel RecoverByUserId(Guid id);

        IEnumerable<AccountModel> RecoverAll();
    }
}
