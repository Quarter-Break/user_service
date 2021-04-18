using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Database.Configurations
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).Length(1, 50);
            RuleFor(user => user.Email).EmailAddress().WithMessage("Email not valid.");
            RuleFor(user => user.Password).Length(8, 50);
            RuleFor(user => user.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage("Password not valid.");
        }
    }
}
