using BicycleCompany.BLL.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace BicycleCompany.BLL.ActionFilters
{
    /// <summary>
    /// A filter that validates object's model and specifies status code returned by action.
    /// </summary>
    public class ValidationFilterAttribute : IActionFilter
    {
        private readonly ILoggerManager _logger;
        public ValidationFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Nothing needed.
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var action = context.RouteData.Values["action"];
            var controller = context.RouteData.Values["controller"];

            var param = context.ActionArguments
                .SingleOrDefault(x => x.Value.ToString().Contains("Model")).Value;
            if (param is null)
            {
                _logger.LogError($"Object sent from client is null. Controller: {controller}, action: {action}");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                _logger.LogInfo($"Invalid model state for the object. Controller: {controller}, action: {action}");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}
