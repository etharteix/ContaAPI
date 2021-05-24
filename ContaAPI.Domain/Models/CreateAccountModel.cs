using System;

namespace ContaAPI.Domain.Models
{
    public class CreateAccountModel
    {
        public CreateAccountModel(decimal balance, Guid userId)
        {
            Balance = balance;
            UserId = userId;
        }

        public decimal Balance { get; set; }

        public Guid UserId { get; set; }
    }
}
