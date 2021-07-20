using System;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public class PartProblem
    {
        public Guid Id { get; set; }

        [Required]
        public Guid PartId { get; set; }
        public Part Part { get; set; }

        [Required]
        public Guid ProblemId { get; set; }
        public Problem Problem { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
