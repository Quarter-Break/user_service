using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserService.Database.Contexts;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Security
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

        public async Task<ActionResult<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            // Check if user exists;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

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
                string token = GenerateJwtToken(user);
                return new AuthenticationResponse(user, token);
            }
            else
            {
                return Unauthorized("Incorrect credentials.");
            }

        }

        // Helper methods
        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        private string GenerateJwtToken(User user)
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

            return tokenHandler.WriteToken(token);
        }
    }
}
