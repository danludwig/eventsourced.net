using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web
{
  public class ReduxServerRenderState
  {
    public ReduxServerRenderState(Controller controller, ReduxAppState app = null) {
      Location = $"{controller.Request.Path}{controller.Request.QueryString}";
      App = app ?? new ReduxAppState(controller.User);
      Routing = new ReduxRoutingState(controller.Request);
    }

    public string Location { get; }
    public ReduxAppState App { get; }
    public ReduxRoutingState Routing { get; }
  }
}
