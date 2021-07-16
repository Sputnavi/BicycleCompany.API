using BicycleCompany.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class BicycleForCreationModel
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
