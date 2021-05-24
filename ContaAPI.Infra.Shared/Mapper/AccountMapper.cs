using System;
using System.Collections.Generic;
using System.Linq;
using ContaAPI.Domain.Entities;
using ContaAPI.Domain.Models;

namespace ContaAPI.Infra.Shared.Mapper
{
    public static class AccountMapper
    {
        public static AccountEntity ConvertToAccountEntity(this CreateAccountModel accountModel) =>
            new AccountEntity(Guid.Empty, accountModel.Balance, accountModel.UserId);

        public static AccountEntity ConvertToAccountEntity(this UpdateAccountModel accountModel, Guid id, Guid userId) =>
            new AccountEntity(id, accountModel.Value, userId);

        public static IEnumerable<AccountModel> ConvertToAccounts(this IList<AccountEntity> accounts) =>
            new List<AccountModel>(accounts.Select(s => new AccountModel(s.Id, s.Balance.ToDecimal(), s.UserId)));

        public static AccountModel ConvertToAccount(this AccountEntity account) =>
            new AccountModel(account.Id, account.Balance.ToDecimal(), account.UserId);
    }
}
