using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public class Bicycle
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
