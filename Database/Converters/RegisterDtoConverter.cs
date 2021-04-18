using Microsoft.AspNetCore.Identity;
using UserService.Database.Models;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Converters
{
    public class RegisterDtoConverter
    {
        public User DtoToModel(RegisterRequest dto)
        {
            PasswordHasher<AuthenticationRequest> passwordHasher = new();
            AuthenticationDtoConverter authenticationRequestConverter = new();

            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                // Hash password before saving to db.
                Password = passwordHasher.HashPassword(authenticationRequestConverter.ModelToDto(dto.Email, dto.Password), dto.Password)
            };
        }

        public UserResponse ModelToDto(User registration)
        {
            return new UserResponse
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
