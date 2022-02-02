using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class BicycleForCreateOrUpdateModel
    {
        /// <summary>
        /// Name of Bicycle
        /// </summary>
        /// <example>LTD</example>
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// Model of Bicycle
        /// </summary>
        /// <example>Junior</example>
        [Required]
        [MaxLength(50)]
        public string Model { get; set; }
    }
}
