using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using indive_test.Data;
using indive_test.Interfaces;
using indive_test.Models.Auth;

namespace indive_test.Repositories
{

    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;

        public AccountRepository(ApplicationDBContext context) => _context = context;

        public async Task<AppUserAccountVerification> CreateAsync(AppUserAccountVerification user)
        {
            await _context.AppUserAccountVerifications.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
    }
}