using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50), MinLength(5)]
        public string Username { get; set; }
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(500)]
        public string Password { get; set; }
        public string AvatarPath { get; set; }
        public bool IsArtist { get; set; }
    }
}
