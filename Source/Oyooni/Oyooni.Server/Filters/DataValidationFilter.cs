using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Oyooni.Server.Common;
using Oyooni.Server.Constants;
using System.Linq;
using System.Threading.Tasks;

namespace Oyooni.Server.Filters
{
    /// <summary>
    /// Represents an asynchronous data validation filter
    /// </summary>
    public class DataValidationFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Handles when the action is executed
        /// </summary>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // If there are errors in the passed model
            if (!context.ModelState.IsValid)
            {
                // Get all errors
                var errors = context.ModelState.Where(v => v.Value.Errors.Any())
                    .ToDictionary(pair => pair.Key, kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToList());
                
                // Set the result to a bad request result along with an api error response along with the errors
                context.Result = new BadRequestObjectResult(
                    new ApiErrorResponse(message: Responses.General.ErrorOccured, errors: errors));

                // Return
                return;
            }

            // Go to the next filter
            await next();
        }
    }
}
