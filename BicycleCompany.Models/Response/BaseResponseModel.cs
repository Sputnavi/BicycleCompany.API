using System.Net;

namespace BicycleCompany.Models.Response
{
    public class BaseResponseModel
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; }

        public BaseResponseModel()
        {

        }

        public BaseResponseModel(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
