using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Dtos.Account
{
    public class VerifyAccountDto
    {
        public required string Email { get; set; }
        public required string VerifyToken { get; set; }

    }
}