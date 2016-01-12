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
  public class ApiController : Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }

    public ApiController(ISendCommand command, IExecuteQuery query) {
      Command = command;
      Query = query;
    }

    [HttpPost, Route("api/login")]
    public async Task<IActionResult> PostLogin([FromBody] LoginRequestModel model) {
      if (model == null) return HttpBadRequest();
      //await Task.Delay(3000);
      await Command.SendAsync(new LogUserIn(model.Login, model.Password, HttpContext.Authentication));
      Response.Headers["Location"] = model.ReturnUrl ?? Url.RouteUrl("HomeRoute");
      Guid? userId = await Query.Execute(new UserIdByLogin(model.Login));
      var claims = await Query.Execute(new ClaimsByUserId(userId.Value));
      return Ok(new {
        Username = claims.Single(x => x.Type == ClaimTypes.Name).Value,
      });
    }
  }
}
