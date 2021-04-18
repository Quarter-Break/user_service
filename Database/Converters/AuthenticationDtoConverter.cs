using UserService.Models;

namespace UserService.Helpers.Security.Models
{
    public class AuthenticationDtoConverter
    {
        public AuthenticationRequest ModelToDto(User user)
        {
            return new AuthenticationRequest
            {
                Email = user.Email,
                Password = user.Password
            };
        }

        public AuthenticationRequest ModelToDto(string email, string password)
        {
            return new AuthenticationRequest
            {
                Email = email,
                Password = password,
            };
        }
    }
}
