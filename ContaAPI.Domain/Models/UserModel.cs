using System;

namespace ContaAPI.Domain.Models
{
    public class UserModel
    {
        public UserModel(Guid id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
