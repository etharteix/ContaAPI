using System;

namespace ContaAPI.Domain.Models
{
    public class UpdateUserModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
