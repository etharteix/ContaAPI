﻿using System;

namespace ContaAPI.Domain.Models
{
    public class UpdateUserModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
