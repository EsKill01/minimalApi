using AutoMapper;
using FluentValidation;
using MagicalVilla_CoponAPI.Models;
using MagicalVilla_CoponAPI.Models.DTO.LocalUser;
using MagicalVilla_CoponAPI.Repository.AuthRepository;
using Microsoft.AspNetCore.Mvc;

namespace MagicalVilla_CoponAPI.EndPoints.AuthEntPoints
{
    public static class CreateUser
    {
        private static async Task<IResult> createUser(IAuthRepository _authRepository,
            IMapper mapper, IValidator<LocalUserRegistrationDTO> validator,
            ILogger<Program> _logger, [FromBody] LocalUserRegistrationDTO localUserRegistrationDTO)
        {
            ApiResponse apiResponse = new ApiResponse();
            _logger.Log(LogLevel.Information, $"Creating user with user name: {localUserRegistrationDTO.UserName}");

            var validationResult = await validator.ValidateAsync(localUserRegistrationDTO);

            if (!validationResult.IsValid)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ErrorMessage);
                return Results.BadRequest(apiResponse);
            }


            var validateUser = await _authRepository.IsUniqueUserAsync(localUserRegistrationDTO.UserName);

            if (validateUser)
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("User name already exists");
                return Results.BadRequest(apiResponse);
            }

            var result = await _authRepository.RegisterAsync(localUserRegistrationDTO);

            if(result == null || string.IsNullOrEmpty(result.UserName))
            {
                apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                apiResponse.IsSuccess = false;
                apiResponse.ErrorMessages.Add("Something goes wrong");
                return Results.BadRequest(apiResponse);
            }

            apiResponse.Result = result;

            return Results.Ok(apiResponse);

      
        }

        public static async void ConfigureCreateUserEndPoint(this WebApplication app)
        {
            app.MapPost("api/auth", createUser)
                .WithName("Create new user")
                .WithDisplayName("Auth")
                .Accepts<LocalUserRegistrationDTO>("application/json")
                .Produces<ApiResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
