﻿using System;

namespace ContaAPI.Domain.Models
{
    public class CreateUserModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}