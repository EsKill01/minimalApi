using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO.Coupon;
using Microsoft.AspNetCore.Mvc;

namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    public static class UpdateCoupon
    {
        private static async Task<IResult> updateCoupon(
            ICouponRepository _couponRepository, 
            IValidator<CouponUpdateDTO> _validator, 
            IMapper _mapper, 
            ILogger<Program> _logger, 
            int id, [FromBody] CouponUpdateDTO model)
        {
            ApiResponse apiResponse = new();
            _logger.Log(LogLevel.Information, $"Put object {id}");

            var couponToUpdate = await _couponRepository.GetAsycn(id);

            if (couponToUpdate == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Coupon do not exists");
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return Results.BadRequest(apiResponse);
            }

            var validationResult = await _validator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add($"Model invalid: {validationResult.Errors.FirstOrDefault().ErrorMessage}");
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

                return Results.BadRequest(apiResponse);
            }

            couponToUpdate.Percent = model.Percent;
            couponToUpdate.IsActive = model.IsActive;
            couponToUpdate.Name = model.Name;
            couponToUpdate.LastUpdate = DateTime.Now;

            //_db.Coupons.Update(_mapper.Map<Coupon>(model));

            await _couponRepository.UpdateAsync(couponToUpdate);

            await _couponRepository.SaveAsync();




            CouponUpDTO upDTO = _mapper.Map<CouponUpDTO>(couponToUpdate);

            apiResponse.Result = upDTO;


            return Results.Ok(apiResponse);
        }
        public static void ConfigureCouponUpdateEndPoint(this WebApplication app)
        {
            app.MapPut("api/coupon/{id:int}", updateCoupon)
                .Accepts<CouponUpdateDTO>("application/json")
                .WithName("UpdateCoupon")
                .Produces<ApiResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest);
        }
    }
}
