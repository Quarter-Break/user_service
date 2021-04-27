using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using UserService.Database.Contexts;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UserContext context) : base(context) { }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await GetAll().FirstOrDefaultAsync(e => e.Email == email);
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await GetAll().FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}
