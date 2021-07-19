using System.ComponentModel.DataAnnotations;

namespace BicycleCompany.DAL.Models
{
    public enum Stage
    {
        [Display(Name = "Received")]
        Received,
        [Display(Name = "On the way")]
        OnTheWay,
        [Display(Name = "In progress")]
        InProgress,
        [Display(Name = "Finished")]
        Finished
    }
}
