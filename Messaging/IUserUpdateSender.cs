using UserService.Database.Models.Dto;

namespace UserService.Messaging
{
    public interface IUserUpdateSender
    {
        void SendUser(UserResponse user);
    }
}
