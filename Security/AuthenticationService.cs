﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using UserService.Database.Contexts;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Helpers.Security
{
    // Source: https://github.com/cornflourblue/aspnet-core-3-jwt-authentication-api/
    public class AuthenticationService : Controller, IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly UserContext _context;
        private readonly PasswordHasher<AuthenticationRequest> passwordHasher;

        public AuthenticationService(IOptions<AppSettings> appSettings, UserContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
            passwordHasher = new();
        }

        public IActionResult Authenticate(AuthenticationRequest request)
        {
            // Check if user exists;
            User user = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            // Return null if user not found
            if (user == null)
            {
                return BadRequest();
            }

            // Check password
            PasswordVerificationResult result = passwordHasher.VerifyHashedPassword(request, user.Password, request.Password);

            if (result.Equals(PasswordVerificationResult.Success))
            {
                // authentication successful so generate jwt token
                return GenerateJwtToken(user);
            }
            else
            {
                return Unauthorized("Incorrect credentials");
            }

        }

        // Helper methods
        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        private IActionResult GenerateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return actionresult with token
            return Ok(tokenHandler.WriteToken(token));
        }
    }
}
