using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Repository.CouponRepository;
using Microsoft.AspNetCore.Authorization;

namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    public static partial class CouponEndPoint
    {
        [Authorize]
        private static async Task<IResult> gelAllCoponds(ICouponRepository _couponRepository, ILogger<Program> _logger)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, "Get all");

            apiResponse.Result = await _couponRepository.GetAllAsycn();


            return Results.Ok(apiResponse);
        }

        [Authorize]
        private static async Task<IResult> getByIdCoupon(ICouponRepository _couponRepository, ILogger<Program> _logger, int id)
        {
            ApiResponse apiResponse = new ApiResponse();

            _logger.Log(LogLevel.Information, $"Get by id: {id}");

            Coupon coupon = await _couponRepository.GetAsycn(id);

            if (coupon == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.ErrorMessages.Add("Object dont exists");

                return Results.BadRequest(apiResponse);
            }

            apiResponse.Result = coupon;

            return Results.Ok(apiResponse);
        }
        public static void ConfigureCouponGetEndPoints(this WebApplication app)
        {
            app.MapGet("api/coupon", gelAllCoponds)
                .WithName("GetAllCoupons")
                .Produces<ApiResponse>(StatusCodes.Status200OK)
                .RequireAuthorization("AdminOnly");

            app.MapGet("api/coupon/{id:int}", getByIdCoupon)
                .WithName("GetCoupon")
                .Produces<ApiResponse>(StatusCodes.Status200OK)
                .RequireAuthorization("AdminOnly");
        }
    }
}
