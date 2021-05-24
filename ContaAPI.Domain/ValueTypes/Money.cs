using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContaAPI.Domain.ValueTypes
{
    public struct Money
    {
        private readonly decimal? _value;
        public readonly Contract contract;

        private Money(decimal? value)
        {
            _value = value;
            contract = new Contract();
            Validate();
        }

        public decimal ToDecimal() =>
            _value.Value;

        public static implicit operator Money(decimal? value) =>
            new Money(value);

        private bool Validate()
        {
            if (_value == null)
                return AddNotification("Please inform a balance value.");

            string strValue = Convert.ToString(_value);
            var splittedValue = strValue.Split(new char[] { ',', '.' });

            if (splittedValue.Length > 1 && splittedValue[1].Length > 2)
                return AddNotification("Balance can't have more than 2 decimal places.");

            if (_value < 0)
                return AddNotification("Balance can't be negative.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Money), message);
            return false;
        }
    }
}
