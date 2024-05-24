using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indive_test.Models.Auth
{
    public class AppUserAccountVerification
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string VerifyToken { get; set; }

    }
}