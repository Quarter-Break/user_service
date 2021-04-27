using UserService.Models;

namespace UserService.Database.Models.Dto
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public AuthenticationRequest() { }

        public AuthenticationRequest(UserRequest request)
        {
            Email = request.Email;
            Password = request.Password;
        }

        public AuthenticationRequest(User user)
        {
            Email = user.Email;
            Password = user.Password;
        }
    }
}
