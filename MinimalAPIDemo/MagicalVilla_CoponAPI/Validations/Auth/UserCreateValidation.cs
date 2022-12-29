using FluentValidation;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

namespace MagicalVilla_CoponAPI.Validations.Auth
{
    public class UserCreateValidation : AbstractValidator<LocalUserRegistrationDTO>
    {
        public UserCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Password).NotEmpty();
            RuleFor(model => model.UserName).NotEmpty();
        }
    }
}
