using System;

namespace BicycleCompany.Models.Response
{
    public class PartForReadModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
    }
}
