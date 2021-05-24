using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContaAPI.Domain.ValueTypes
{
    public struct Name
    {
        private readonly string _value;
        public readonly Contract contract;

        private Name(string value)
        {
            _value = value;
            contract = new Contract();
            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Name(string value) =>
            new Name(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Please inform a name.");

            if (_value.Length < 3)
                return AddNotification("The name must be at least 3 characters in length.");

            if (!Regex.IsMatch(_value, (@"^[A-Za-z][A-Za-z0-9 ]+$")))
                return AddNotification("The name must begin with a letter and have no special character.");

            if (_value.EndsWith(" "))
                return AddNotification("Invalid white space in the end of the name.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Name), message);
            return false;
        }
    }
}
