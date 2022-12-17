namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    using AutoMapper;
    using FluentValidation;
    using MagicalVilla_CoponAPI.models;
    using MagicalVilla_CoponAPI.Models;
    using MagicalVilla_CoponAPI.Models.DTO.Coupon;
    using MagicalVilla_CoponAPI.Repository;
    using Microsoft.AspNetCore.Mvc;

    public static class PostCoupon
    {
        private static async Task<IResult> postCoupon(
            ICouponRepository _couponRepository, 
            IMapper _mapper,
            IValidator<CouponCreateDTO> _validator,
            ILogger<Program> _logger,
            [FromBody] CouponCreateDTO couponCreateDTO)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, "Post object");

            var validationResult = await _validator.ValidateAsync(couponCreateDTO);
            var existName = await _couponRepository.GetAsycn(couponCreateDTO.Name);

            if (!validationResult.IsValid)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
                return Results.BadRequest(apiResponse);
            }
            else if (existName != null)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
                return Results.BadRequest("Coupon allready exists");
            }

            Coupon coupon = _mapper.Map<Coupon>(couponCreateDTO);

            await _couponRepository.CreateAsync(coupon);
            await _couponRepository.SaveAsync();


            apiResponse.Result = coupon;

            return Results.CreatedAtRoute($"GetCoupon", new
            {
                Id = coupon.Id,
            },
            apiResponse);
        }
        public static void ConfigurePostCouponEndPoint(this WebApplication app)
        {
            app.MapPost("api/coupon", postCoupon)
                .WithName("Add coupon")
                .Accepts<CouponCreateDTO>("application/json")
                .Produces<ApiResponse>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
