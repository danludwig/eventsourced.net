using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EventSourced.Net.Services.Web.Mvc
{
  [UsedImplicitly]
  public class ExecutionContextScopeMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly Container _container;

    public ExecutionContextScopeMiddleware(RequestDelegate next, Container container) {
      _next = next;
      _container = container;
    }

    public async Task Invoke(HttpContext context) {
      using (_container.BeginExecutionContextScope()) {
        await _next.Invoke(context).ConfigureAwait(false);
      }
    }
  }
}
