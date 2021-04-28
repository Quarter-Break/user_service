using Microsoft.AspNetCore.Identity;
using UserService.Database.Models.Dto;
using UserService.Models;

namespace UserService.Database.Converters
{
    public class UpdateDtoConverter
    {
        public User DtoToModel(UpdateRequest dto, User user)
        {
            PasswordHasher<AuthenticationRequest> passwordHasher = new();
            User updatedUser = user;

            if (dto.Username != null)
            {
                updatedUser.Username = dto.Username;
            }
            if (dto.Password != null)
            {
                updatedUser.Password = passwordHasher.HashPassword(new AuthenticationRequest(user), dto.Password);
            }
            if (dto.AvatarPath != null)
            {
                updatedUser.AvatarPath = dto.AvatarPath;
            }

            return updatedUser;
        }
    }
}
