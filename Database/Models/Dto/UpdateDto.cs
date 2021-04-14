using System;
using System.ComponentModel.DataAnnotations;

namespace UserService.Database.Models.Dto
{
    public class UpdateDto
    {
        public string Username { get; set; }
        [MaxLength(50), MinLength(8)]
        public string Password { get; set; }
        public string AvatarPath { get; set; }
    }
}
