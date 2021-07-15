using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
