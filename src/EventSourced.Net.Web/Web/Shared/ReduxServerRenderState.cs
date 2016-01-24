using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web
{
  public class ReduxServerRenderState
  {
    public ReduxServerRenderState(Controller controller) {
      Location = $"{controller.Request.Path}{controller.Request.QueryString}";
      App = new ReduxAppState(controller.User);
      Routing = new ReduxRoutingState(controller.Request);
    }

    public ReduxAppState App { get; }
    public string Location { get; }
    public ReduxRoutingState Routing { get; }
  }

  public class ReduxRoutingState
  {
    public ReduxRoutingState(HttpRequest request) {
      Location = new ReduxRoutingLocationState(request);
    }

    public ReduxRoutingLocationState Location { get; }
  }

  public class ReduxRoutingLocationState
  {
    public ReduxRoutingLocationState(HttpRequest request) {
      Query = request.Query.ToDictionary(x => x.Key, x => x.Value);
      Search = request.QueryString.ToString();
    }

    public object Query { get; }
    public string Search { get; }
  }
}
