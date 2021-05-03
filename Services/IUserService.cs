using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        Task<ActionResult<AuthenticationResponse>> AddAsync(UserRequest request);
        Task<ActionResult<AuthenticationResponse>> Login(AuthenticationRequest request);
        Task<ActionResult<UserResponse>> GetByIdAsync(Guid id);
        Task<ActionResult<UserResponse>> GetByEmailAsync(string email);
        Task<ActionResult<UserResponse>> UpdateAsync(Guid id, UpdateRequest request);
        Task<ActionResult<UserResponse>> DeleteByIdAsync(Guid id);
    }
}
