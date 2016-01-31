using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web
{
  public class ServerRenderedAppViewResult : ViewResult
  {
    public ServerRenderedAppViewResult(Controller controller, string pageTitle, ReduxServerRenderState model = null)
    {
      controller.ViewData["Title"] = pageTitle;
      model = model ?? new ReduxServerRenderState(controller);
      controller.ViewData.Model = model;
      ViewName = "~/Web/Shared/ServerRenderedApp.cshtml";
      ViewData = controller.ViewData;
      TempData = controller.TempData;
    }
  }
}
