using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Repository;

namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    public static class GetCoupon
    {
        public static void ConfigureCouponGetEndPoints(this WebApplication app)
        {
            app.MapGet("api/coupon", async (ICouponRepository _couponRepository, ILogger<Program> _logger) =>
            {
                ApiResponse apiResponse = new ApiResponse();
                _logger.Log(LogLevel.Information, "Get all");

                apiResponse.Result = await _couponRepository.GetAllAsycn();


                return Results.Ok(apiResponse);
            }).WithName("GetAllCoupons").Produces<ApiResponse>(StatusCodes.Status200OK);

            app.MapGet("api/coupon/{id:int}", async (ICouponRepository _couponRepository, ILogger<Program> _logger, int id) =>
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

            }).WithName("GetCoupon").Produces<ApiResponse>(StatusCodes.Status200OK);
        }
    }
}
