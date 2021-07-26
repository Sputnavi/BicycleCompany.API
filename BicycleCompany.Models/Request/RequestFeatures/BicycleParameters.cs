namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class BicycleParameters : RequestParameters
    {
        public BicycleParameters()
        {
            OrderBy = "name";
        }

        public string SearchTerm { get; set; }
    }
}
