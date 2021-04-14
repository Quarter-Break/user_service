using Microsoft.AspNetCore.Identity;
using System;
using UserService.Database.Models.Dto;
using UserService.Helpers.Security.Models;
using UserService.Models;

namespace UserService.Helpers.Converters
{
    public class UpdateDtoConverter
    {
        public User DtoToModel(UpdateDto dto, User user)
        {
            PasswordHasher<AuthenticationRequest> passwordHasher = new();
            AuthenticationRequestConverter authenticationRequestConverter = new();
            User updatedUser = user;

            if (dto.Username != null)
            {
                updatedUser.Username = dto.Username;
            }
            if (dto.Password != null)
            {
                updatedUser.Password = passwordHasher.HashPassword(authenticationRequestConverter.UserToRequest(user), dto.Password);
            }
            if (dto.AvatarPath != null)
            {
                updatedUser.AvatarPath = dto.AvatarPath;
            }

            return updatedUser;
        }
    }
}
