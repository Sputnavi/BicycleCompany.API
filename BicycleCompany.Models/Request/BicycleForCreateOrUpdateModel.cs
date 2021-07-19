using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class BicycleForCreateOrUpdateModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }
    }
}
