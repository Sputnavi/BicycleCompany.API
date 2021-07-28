namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class PartParameters : RequestParameters
    {
        public PartParameters()
        {
            OrderBy = "name";
        }

        public string SearchTerm { get; set; }
    }
}
