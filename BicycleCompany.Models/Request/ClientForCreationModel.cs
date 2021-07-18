using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class ClientForCreationModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public List<Problem> Problems { get; set; }
    }
}
