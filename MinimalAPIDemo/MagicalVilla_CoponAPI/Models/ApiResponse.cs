using System.Net;

namespace MagicalVilla_CoponAPI.Models
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            ErrorMessages= new List<string>();
        }

        public bool IsSuccess { get; set; } = true;

        public Object Result { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;

        public List<string> ErrorMessages { get; set; }

        public void SetFailResponse(List<string> errorMessages, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            ErrorMessages = errorMessages;
            StatusCode = statusCode;
            IsSuccess = false;
        }

    }
}
