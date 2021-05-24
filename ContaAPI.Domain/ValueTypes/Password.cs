using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContaAPI.Domain.ValueTypes
{
    public struct Password
    {
        private readonly string _value;
        public readonly Contract contract;

        private Password(string value)
        {
            _value = value;
            contract = new Contract();
            Validate();

            if (contract.Valid)
                _value = Convert.ToBase64String(new UTF8Encoding().GetBytes(_value));
        }

        public override string ToString() =>
            _value;

        public static implicit operator Password(string input) =>
            new Password(input.Trim());

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Please inform the password.");

            if (_value.Length < 6)
                return AddNotification("The password must be at least 6 characters in length.");

            if (!Regex.IsMatch(_value, (@"^.*[a-zA-Z].*$")))
                return AddNotification("The password must have at least one letter.");

            if (!Regex.IsMatch(_value, (@"^.*[0-9].*$")))
                return AddNotification("The password must have at least one number.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Password), message);
            return false;
        }
    }
}
