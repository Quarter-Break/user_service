using System;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
    }
}
