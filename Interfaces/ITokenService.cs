using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indive_test.Models;

namespace indive_test.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
        string CreateRandomToken();
    }
}