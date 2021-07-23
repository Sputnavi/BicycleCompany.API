using System;
using System.Collections.Generic;

namespace BicycleCompany.Models.Response
{
    public class ProblemForReadModel
    {
        public Guid Id { get; set; }

        public BicycleForReadModel Bicycle { get; set; }

        public DateTime Date { get; set; }

        public string Place { get; set; }

        public int Stage { get; set; }

        public string Description { get; set; }

        public List<PartProblemForReadModel> Parts { get; set; }
    }
}
