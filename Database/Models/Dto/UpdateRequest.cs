namespace UserService.Database.Models.Dto
{
    public class UpdateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarPath { get; set; }
    }
}
