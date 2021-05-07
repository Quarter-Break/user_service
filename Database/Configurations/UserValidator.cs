using FluentValidation;
using UserService.Database.Models.Dto;

namespace UserService.Database.Configurations
{
    public class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).Length(1, 50);
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email not valid.");
            RuleFor(user => user.Password).Length(8, 50).WithMessage("Password must contain between 8 and 50 characters.");
            // THIS REGEX DOES NOT ACCEPT SPECIAL CHARACTERS. FIX
            RuleFor(user => user.Password).Matches(@"^(.*[A-Z].*)$").WithMessage("Password must contain at least one capital letter.");
            RuleFor(user => user.Password).Matches(@"^(.*\d.*)$").WithMessage("Password must contain at least one digit.");
        }
    }
}
