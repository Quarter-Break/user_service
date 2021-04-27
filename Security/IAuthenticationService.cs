using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Security
{
    // Source: https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api/
    public interface IAuthenticationService
    {
        Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        User GetById(Guid id);
    }
}
