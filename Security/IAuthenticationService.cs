using Microsoft.AspNetCore.Mvc;
using System;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Helpers.Security
{
    // Source: https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api/
    public interface IAuthenticationService
    {
        IActionResult Authenticate(AuthenticationRequest request);
        User GetById(Guid id);
    }
}
