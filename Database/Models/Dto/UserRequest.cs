using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models.Dto
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
