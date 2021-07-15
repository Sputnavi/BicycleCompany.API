using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public class Part
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
