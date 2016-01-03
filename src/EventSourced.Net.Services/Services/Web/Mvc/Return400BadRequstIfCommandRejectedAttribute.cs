using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Filters;

namespace EventSourced.Net.Services.Web.Mvc
{
  public class Return400BadRequstIfCommandRejectedAttribute : ActionFilterAttribute
  {
    public override void OnActionExecuted(ActionExecutedContext context) {
      var exception = context.Exception as CommandRejectedException;
      if (exception == null) return;

      context.Result = new BadRequestObjectResult(exception.Errors);
      context.ExceptionHandled = true;
    }
  }
}