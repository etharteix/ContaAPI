using ContaAPI.Domain.ValueTypes;
using Flunt.Validations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ContaAPI.Domain.Entities
{
    public class AccountEntity : BaseEntity<Guid>
    {
        public AccountEntity(Guid id, Money balance, Guid userId) : base(id)
        {
            AddNotifications(balance.contract);

            if (Valid)
            {
                Balance = balance;
                UserId = userId;
            }
        }

        public AccountEntity(Guid id, Money balance) : base(id)
        {
            AddNotifications(balance.contract);

            if (Valid)
                Balance = balance;
        }

        protected AccountEntity() { }

        public Money Balance { get; set; }

        public Guid UserId { get; }
    }
}