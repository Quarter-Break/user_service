using System.ComponentModel.DataAnnotations;

namespace UserService.Models
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(50), MinLength(4)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [MaxLength(50), MinLength(8)]
        public string Password { get; set; }
    }
}
