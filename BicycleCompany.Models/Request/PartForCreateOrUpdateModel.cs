using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class PartForCreateOrUpdateModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
