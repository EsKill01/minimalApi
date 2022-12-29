using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;
using MagicalVilla_CoponAPI.Repository.AuthRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicalVilla_CoponAPI.EndPoints.AuthEntPoints
{
    public static class LoginUser
    {
        private static async Task<IResult> loginUser(IAuthRepository _authRepository, 
            IValidator<LocalUserLoginDTO> _validator, 
            ILogger<Program> _logger, 
            [FromBody] LocalUserLoginDTO localUserLoginDTO)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, $"Login user: {localUserLoginDTO.UserName}");

            var validationResult = await _validator.ValidateAsync(localUserLoginDTO);

            if (!validationResult.IsValid)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess= false;
                apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
                return Results.BadRequest(apiResponse);
            }
            
            var isAuthenticated = await _authRepository.AuthenticateAsync(localUserLoginDTO);

            if(isAuthenticated== null)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("User name or password is incorrect");
                return Results.BadRequest(apiResponse);
            }

            apiResponse.Result = isAuthenticated;

            return Results.Ok(apiResponse);
        }


        public static async void ConfigureLoginEndPoint(this WebApplication app)
        {
            app.MapPost("api/login", loginUser)
               .WithDisplayName("Login user")
               .Accepts<LocalUserLoginDTO>("application/json")
               .Produces<ApiResponse>(StatusCodes.Status201Created)
               .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
