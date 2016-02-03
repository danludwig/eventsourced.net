using Microsoft.AspNet.Http;

namespace EventSourced.Net.Web
{
  public class ReduxRoutingState
  {
    public ReduxRoutingState(HttpRequest request) {
      Location = new ReduxRoutingLocationState(request);
    }

    public ReduxRoutingLocationState Location { get; }
  }
}
