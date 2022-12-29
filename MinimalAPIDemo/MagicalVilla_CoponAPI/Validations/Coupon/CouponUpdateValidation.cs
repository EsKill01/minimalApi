using FluentValidation;
using MagicalVilla_CoponAPI.Models.DTO.Coupon;

namespace MagicalVilla_CoponAPI.Validations.Coupon
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
            RuleFor(model => model.Id).GreaterThan(0);
        }
    }
}
