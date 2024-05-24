using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Dtos.Account
{
    public class LoginDto
    {
        public required string Username { get; set; }

        public required string Password { get; set; }


    }
}