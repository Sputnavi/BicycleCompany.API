namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class PartParameters : RequestParameters
    {
        public PartParameters()
        {
            OrderBy = "name";
        }

        public string SearchTerm { get; set; }
        public int MinAmount { get; set; }
        public int MaxAmount { get; set; } = int.MaxValue;

        public bool ValidAmountRange => MaxAmount >= MinAmount;
    }
}
