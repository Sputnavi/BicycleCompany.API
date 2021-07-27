using System;

namespace BicycleCompany.Models.Response
{
    public class PartDetailsForReadModel
    {
        public Guid Id { get; set; }
        public PartForReadModel Part { get; set; }
        public int Amount { get; set; }
    }
}
