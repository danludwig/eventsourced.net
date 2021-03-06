﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Login
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
    public IActionResult GetView() {
      return new ServerRenderedAppViewResult(this, "Login");
    }

    [HttpPost, Route("api/login")]
    public async Task<IActionResult> PostApi([FromBody] PostApiRequest model, string returnUrl) {
      //return HttpNotFound();
      if (model == null) return HttpBadRequest(new object());
      //await Task.Delay(1000);
      await Command.SendAsync(new LogUserIn(model.Login, model.Password, HttpContext.Authentication));
      Response.Headers["Location"] = returnUrl ?? Url.RouteUrl("HomeRoute");
      Guid? userId = await Query.Execute(new UserIdByLogin(model.Login));
      var claims = await Query.Execute(new ClaimsByUserId(userId.Value));
      return Ok(new PostApiResponse {
        Username = claims.Single(x => x.Type == ClaimTypes.Name).Value,
      });
    }
  }
}
