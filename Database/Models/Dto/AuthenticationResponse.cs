using System;
using UserService.Models;

namespace UserService.Database.Models.Dto
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }

        public AuthenticationResponse(User user, string token)
        {
            Id = user.Id;
            Username = user.Username;
            Token = token;
        }
    }
}
