using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class ProblemForCreateModel
    {
        [Required]
        public Guid BicycleId { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(200)]
        public string Place { get; set; }

        [Required]
        public int Stage { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public List<PartProblemForCreateModel> Parts { get; set; }
    }
}
