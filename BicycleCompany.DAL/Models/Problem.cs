using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BicycleCompany.DAL.Models
{
    public class Problem
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Client))]
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        [ForeignKey(nameof(Bicycle))]
        public Guid BicycleId { get; set; }
        public Bicycle Bicycle { get; set; }

        [Required]
        [MaxLength(200)]
        public string Place { get; set; }

        // Default Value "Received" Configured in Migration
        [Required]
        [MaxLength(20)]
        public string Stage { get; set; }

        [MaxLength(250)]
        public string Description { get; set; }

        public List<Part> Parts { get; set; }
    }
}
