using System;
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
    private RequestDelegate Next { get; }
    private Container Container { get; }

    public ExecutionContextScopeMiddleware(RequestDelegate next, Container container) {
      Next = next;
      Container = container;
    }

    public async Task Invoke(HttpContext context) {
      using (Container.BeginExecutionContextScope()) {
        try {
          await Next.Invoke(context).ConfigureAwait(false);
        } catch (Exception ex) {
          Console.WriteLine(ex.ToString());
          throw;
        }
      }
    }
  }
}
