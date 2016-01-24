using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Login
{
  [Return400BadRequstIfCommandRejected]
  public class Controller : Microsoft.AspNet.Mvc.Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }

    public Controller(ISendCommand command, IExecuteQuery query) {
      Command = command;
      Query = query;
    }

    [HttpGet, Route("login", Name = "LoginRoute")]
    public IActionResult ViewLogin() {
      var model = this.BuildServerRenderReduxState();
      model.App.Ui.Login = new object();
      return this.ServerRenderedView("Login", model);
    }

    [HttpPost, Route("api/login")]
    public async Task<IActionResult> PostLogin([FromBody] PostLoginRequest model, string returnUrl) {
      if (model == null) return HttpBadRequest();
      //await Task.Delay(1000);
      await Command.SendAsync(new LogUserIn(model.Login, model.Password, HttpContext.Authentication));
      Response.Headers["Location"] = returnUrl ?? Url.RouteUrl("HomeRoute");
      Guid? userId = await Query.Execute(new UserIdByLogin(model.Login));
      var claims = await Query.Execute(new ClaimsByUserId(userId.Value));
      return Ok(new PostLoginResponse {
        Username = claims.Single(x => x.Type == ClaimTypes.Name).Value,
      });
    }

    [HttpPost, Route("logoff")]
    public async Task<IActionResult> Logoff() {
      await Command.SendAsync(new LogUserOff(HttpContext.Authentication));
      return Redirect("~/");
    }
  }
}
