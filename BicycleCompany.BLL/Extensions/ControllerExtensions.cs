using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BicycleCompany.BLL.Extensions
{
    public static class ControllerExtensions
    {
        public static void ValidateObject(this ControllerBase controller)
        {
            if (!controller.ModelState.IsValid)
            {
                throw new ArgumentException(string.Join(", ", controller.ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage)));
            }
        }
    }
}
