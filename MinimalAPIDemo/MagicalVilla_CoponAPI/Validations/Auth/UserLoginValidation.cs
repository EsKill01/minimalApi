using FluentValidation;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;

namespace MagicalVilla_CoponAPI.Validations.Auth
{
    public class UserLoginValidation : AbstractValidator<LocalUserLoginDTO>
    {
        public UserLoginValidation()
        {
            RuleFor(model => model.UserName).NotEmpty();
            RuleFor(model => model.Password).NotEmpty();
        }
    }
}
