using System;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
        
        public string Role { get; set; }
    }
}
