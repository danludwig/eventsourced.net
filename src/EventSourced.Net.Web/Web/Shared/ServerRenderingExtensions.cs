using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web
{
  public static class ServerRenderingExtensions
  {
    public static ViewResult ServerRenderedView(this Controller controller, string pageTitle, ReduxServerRenderState model = null)
    {
      controller.ViewData["Title"] = pageTitle;
      model = model ?? controller.BuildServerRenderReduxState();
      return controller.View("~/Web/Shared/ServerRenderedApp.cshtml", model);
    }

    public static ReduxServerRenderState BuildServerRenderReduxState(this Controller controller) {
      return new ReduxServerRenderState(controller);
    }
  }
}
