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
}
