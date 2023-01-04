using MagicalVilla_CoponAPI.Models;
using System.Net;

namespace MagicalVilla_CoponAPI.Filters
{
    public class ParameterIDValidator : IEndpointFilter
    {
        private ILogger<ParameterIDValidator> _logger;

        public ParameterIDValidator(ILogger<ParameterIDValidator> logger)
        {
            _logger = logger;
        }

        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(int)) as int?;

            if(id == null || id == 0)
            {
                ApiResponse response = new ApiResponse();

                response.SetFailResponse(new List<string>()
                {
                    "Id cannot be zero."
                });
                _logger.Log(LogLevel.Error, "ID cannot be 0");

                return Results.BadRequest(response);
            }

            return await next(context);
        }
    }
}
