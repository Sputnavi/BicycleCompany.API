using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.Models.Request
{
    public class ClientForCreateOrUpdateModel
    {
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        public List<ProblemForCreateModel> Problems { get; set; }
    }
}
