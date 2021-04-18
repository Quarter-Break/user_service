using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Helpers.Security.Models
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
