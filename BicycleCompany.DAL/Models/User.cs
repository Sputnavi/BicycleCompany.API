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
        [MaxLength(44)]
        public string Password { get; set; }

        [Required]
        [MaxLength(24)]
        public string Salt { get; set; }

        public string Role { get; set; }
    }
}
