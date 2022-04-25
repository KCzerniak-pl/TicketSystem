using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TicketSystemWebApp.Helpers
{
    // Filter executed in case of incorrect validation of the security token for forms.
    public class ValidateAntiForgeryTokenFailed : IAlwaysRunResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context) { }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is IAntiforgeryValidationFailedResult)
            {
                context.Result = new RedirectResult("/error");
            }
        }
    }
}
