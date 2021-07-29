using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class UserForRegistrationModel
    {
        [Required]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
