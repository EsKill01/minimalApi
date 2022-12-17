using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Repository;

namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    public static class GetCoupon
    {
        private static async Task<IResult> gelAllCoponds(ICouponRepository _couponRepository, ILogger<Program> _logger)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, "Get all");

            apiResponse.Result = await _couponRepository.GetAllAsycn();


            return Results.Ok(apiResponse);
        }

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
                .Produces<ApiResponse>(StatusCodes.Status200OK);

            app.MapGet("api/coupon/{id:int}", getByIdCoupon)
                .WithName("GetCoupon")
                .Produces<ApiResponse>(StatusCodes.Status200OK);
        }
    }
}
