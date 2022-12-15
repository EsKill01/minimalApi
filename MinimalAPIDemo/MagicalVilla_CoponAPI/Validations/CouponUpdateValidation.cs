using FluentValidation;
using MagicalVilla_CoponAPI.Models.DTO;

namespace MagicalVilla_CoponAPI.Validations
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
