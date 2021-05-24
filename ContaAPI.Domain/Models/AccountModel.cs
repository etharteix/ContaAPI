using System;

namespace ContaAPI.Domain.Models
{
    public class AccountModel
    {
        public AccountModel(Guid id, decimal balance, Guid userId)
        {
            Id = id;
            Balance = balance;
            UserId = userId;
        }

        public Guid Id { get; set; }

        public decimal Balance { get; set; }

        public Guid UserId { get; set; }
    }
}
