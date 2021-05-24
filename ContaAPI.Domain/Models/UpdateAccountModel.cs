using System;

namespace ContaAPI.Domain.Models
{
    public class UpdateAccountModel
    {
        public UpdateAccountModel(decimal value)
        {
            Value = value;
        }

        public decimal Value { get; set; }
    }
}
