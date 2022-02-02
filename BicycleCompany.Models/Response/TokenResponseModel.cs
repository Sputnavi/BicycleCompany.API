namespace BicycleCompany.Models.Response
{
    public class TokenResponseModel
    {
        public string Token { get; set; }

        public TokenResponseModel(string token)
        {
            Token = token;
        }
    }
}
