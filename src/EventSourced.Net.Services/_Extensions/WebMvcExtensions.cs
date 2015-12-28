using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Builder;

namespace EventSourced.Net
{
  public static class WebMvcExtensions
  {
    public static IApplicationBuilder UseExecutionContextScope(this IApplicationBuilder builder) {
      return builder.UseMiddleware<ExecutionContextScopeMiddleware>();
    }
  }
}