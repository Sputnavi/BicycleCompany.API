using System;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class PartProblemForCreateModel
    {
        [Required]
        public Guid PartId { get; set; }

        [Range(1, int.MaxValue)]
        public int Amount { get; set; }
    }
}
