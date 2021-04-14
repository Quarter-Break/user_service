using UserService.Models;

namespace UserService.Helpers.Security.Models
{
    public class AuthenticationRequestConverter
    {
        public AuthenticationRequest UserToRequest(User user)
        {
            return new AuthenticationRequest
            {
                Email = user.Email,
                Password = user.Password
            };
        }

        public AuthenticationRequest DtoToRequest(string email, string password)
        {
            return new AuthenticationRequest
            {
                Email = email,
                Password = password,
            };
        }
    }
}
