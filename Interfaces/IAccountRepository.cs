using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indive_test.Models.Auth;

namespace indive_test.Interfaces
{
    public interface IAccountRepository
    {
        Task<AppUserAccountVerification> CreateAsync(AppUserAccountVerification user);
        
    }
}