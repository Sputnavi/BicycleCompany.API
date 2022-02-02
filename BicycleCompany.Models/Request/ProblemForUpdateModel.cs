using System;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class ProblemForUpdateModel
    {
        public Guid BicycleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Place { get; set; }

        [Range(1, 3)]
        public Stage Stage { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }
    }
}
