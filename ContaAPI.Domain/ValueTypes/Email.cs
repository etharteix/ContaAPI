using Flunt.Validations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ContaAPI.Domain.ValueTypes
{
    public struct Email
    {
        private readonly string _value;
        public readonly Contract contract;

        private Email(string value)
        {
            _value = value;
            contract = new Contract();
            Validate();
        }

        public override string ToString() =>
            _value;

        public static implicit operator Email(string value) =>
            new Email(value);

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(_value))
                return AddNotification("Please inform the email.");

            if (!Regex.IsMatch(_value, (@"^[a-z0-9.]+@[a-z0-9]+\.+[a-z]+?(\.([a-z]+))?$")))
                return AddNotification("Invalid email address.");

            return true;
        }

        private bool AddNotification(string message)
        {
            contract.AddNotification(nameof(Email), message);
            return false;
        }
    }
}
