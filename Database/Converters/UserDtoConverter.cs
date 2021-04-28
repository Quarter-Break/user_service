using Microsoft.AspNetCore.Identity;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Database.Converters
{
    public class UserDtoConverter : IDtoConverter<User, UserRequest, UserResponse>
    {
        public User DtoToModel(UserRequest dto)
        {
            PasswordHasher<AuthenticationRequest> passwordHasher = new();

            return new User
            {
                Username = dto.Username,
                Email = dto.Email,
                // Hash password before saving to db.
                Password = passwordHasher.HashPassword(new AuthenticationRequest(dto), dto.Password)
            };
        }

        public UserResponse ModelToDto(User model)
        {
            return new UserResponse
            {
                Id = model.Id,
                Username = model.Username,
                Email = model.Email,
                AvatarPath = model.AvatarPath,
                IsArtist = model.IsArtist
            };
        }
    }
}
