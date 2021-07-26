namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class ProblemParameters : RequestParameters
    {
        public ProblemParameters()
        {
            OrderBy = "stage";
        }

        public string SearchTerm { get; set; }
    }
}
