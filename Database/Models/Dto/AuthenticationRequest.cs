using System.ComponentModel.DataAnnotations;
using UserService.Models;

namespace UserService.Helpers.Security.Models
{
    public class AuthenticationRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
