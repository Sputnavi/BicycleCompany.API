namespace BicycleCompany.Models.Response
{
    public class ErrorResponseModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ErrorResponseModel(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
