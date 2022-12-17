using MagicalVilla_CoponAPI.models;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Repository;

namespace MagicalVilla_CoponAPI.EndPoints.CouponEndPoints
{
    public static class DeleteCoupon
    {
        private async static Task<IResult> deleteCoupon(ICouponRepository _couponRepository, ILogger<Program> _logger, int id)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, $"Delete object {id}");

            Coupon coupon = await _couponRepository.GetAsycn(id);

            if (coupon == null)
            {
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Object do not exits");
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;

                return Results.BadRequest(apiResponse);
            }

            apiResponse.Result = "Object deleted";

            await _couponRepository.RemoveAsync(coupon);

            await _couponRepository.SaveAsync();

            return Results.Ok(apiResponse);
        }
        public static void ConfigureDeleteCouponEndPoint(this WebApplication app)
        { 

            app.MapDelete("api/coupon/{id:int}", deleteCoupon)
                .WithName("DeleteCoupon")
                .Produces<ApiResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}