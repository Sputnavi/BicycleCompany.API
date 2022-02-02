using System;

namespace BicycleCompany.Models.Response
{
    public class UserForReadModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }
}
