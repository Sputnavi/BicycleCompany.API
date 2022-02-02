using System;

namespace BicycleCompany.Models.Response
{
    public class BicycleForReadModel
    {
        /// <summary>
        /// Bicycle id
        /// </summary>
        /// <example>D9F9841A-AACF-4BC4-924C-04C46E8274F1</example>
        public Guid Id { get; set; }
        /// <summary>
        /// Bicycle name
        /// </summary>
        /// <example>Aist</example>
        public string Name { get; set; }
        /// <summary>
        /// Bicycle model
        /// </summary>
        /// <example>Tango</example>
        public string Model { get; set; }
    }
}
