using ContaAPI.Domain.ValueTypes;
using Flunt.Validations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ContaAPI.Domain.Entities
{
    public class UserEntity : BaseEntity<Guid>
    {
        public UserEntity(Guid id, Name name, Email email, Password password) : base(id)
        {
            AddNotifications(
                name.contract,
                email.contract,
                password.contract);

            if (Valid)
            {
                Name = name;
                Email = email;
                Password = password;
            }
        }

        public UserEntity(Guid id, Name name, Email email) : base(id)
        {
            AddNotifications(
                name.contract,
                email.contract);

            if (Valid)
            {
                Name = name;
                Email = email;
            }
        }

        protected UserEntity() { }

        public Name Name { get; }

        public Email Email { get; }

        public Password Password { get; }
    }
}