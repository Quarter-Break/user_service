using System;

namespace UserService.Database.Models.Dto
{
    public class UserResponse
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string AvatarPath { get; set; }
        public bool IsArtist { get; set; }
    }
}
