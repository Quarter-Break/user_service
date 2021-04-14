using Microsoft.AspNetCore.Identity;
using UserService.Database.Models;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Converters
{
    public class RegisterDtoConverter
    {
        public User DtoToModel(RegisterDto dto)
        {
            PasswordHasher<AuthenticationRequest> passwordHasher = new();
            AuthenticationRequestConverter authenticationRequestConverter = new();

            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                // Hash password before saving to db.
                Password = passwordHasher.HashPassword(authenticationRequestConverter.DtoToRequest(dto.Email, dto.Password), dto.Password)
            };
        }

        public UserDto ModelToDto(User registration)
        {
            return new UserDto
            {
                Id = registration.Id,
                Username = registration.Username,
                Email = registration.Email,
                AvatarPath = registration.AvatarPath,
                IsArtist = registration.IsArtist
            };
        }
    }
}
