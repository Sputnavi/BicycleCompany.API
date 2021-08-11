namespace BicycleCompany.Models.Request.RequestFeatures
{
    public class BicycleParameters : RequestParameters
    {
        public BicycleParameters()
        {
            OrderBy = "name";
        }

        /// <summary>
        /// The string which is used to find bicycle by name.
        /// </summary>
        /// <example>Aist</example>
        public string SearchTerm { get; set; }
    }
}
