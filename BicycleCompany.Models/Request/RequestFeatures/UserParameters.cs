namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class UserParameters : RequestParameters
    {
        public UserParameters()
        {
            OrderBy = "name";
        }
        public string SearchTerm { get; set; }
    }
}
