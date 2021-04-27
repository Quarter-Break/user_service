using System;
using System.Threading.Tasks;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<User> AddUserAsync(UserRequest request);
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> UpdateUserAsync(Guid id, UpdateRequest request);
        Task<User> DeleteUserByIdAsync(Guid id);
    }
}
