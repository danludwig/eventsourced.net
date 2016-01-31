//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using EventSourced.Net.ReadModel.Users;
//using EventSourced.Net.Services.Web.Mvc;
//using EventSourced.Net.Services.Web.Sockets;
//using Microsoft.AspNet.Mvc;
//
//namespace EventSourced.Net.Web.Users.Register
//{
//  [Return400BadRequstIfCommandRejected]
//  public class Controller : Microsoft.AspNet.Mvc.Controller
//  {
//    private ISendCommand Command { get; }
//    private IExecuteQuery Query { get; }
//    private IServeWebSockets WebSockets { get; }
//
//    public Controller(ISendCommand command, IExecuteQuery query, IServeWebSockets webSockets) {
//      Command = command;
//      Query = query;
//      WebSockets = webSockets;
//    }
//
//    //    [HttpGet, Route("register/{correlationId}/redeem", Name = "RegisterRedeemRoute")]
//    //    public async Task<IActionResult> ViewRedeem(string correlationId, string token) {
//    //      Guid correlationGuid;
//    //      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
//    //
//    //      UserContactChallengeRedeemData data = await Query
//    //        .Execute(new UserContactChallengeRedeemView(correlationGuid, token));
//    //      if (data == null) return HttpNotFound();
//    //      var model = this.BuildServerRenderReduxState();
//    //      model.App.Ui.Redeem = new ReduxUiRedeemState(data);
//    //      model.App.Ui.CheckUsername = new object();
//    //      return this.ServerRenderedView("Create login", model);
//    //    }
//    //
//    //    [HttpPost, Route("api/register/{correlationId}/redeem")]
//    //    public async Task<IActionResult> PostRedeem(string correlationId, string token, [FromBody] PostRedeemRequest model) {
//    //      if (model == null) return HttpBadRequest(new object());
//    //      Guid correlationGuid;
//    //      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
//    //
//    //      UserContactChallengeRedeemData data = await Query
//    //        .Execute(new UserContactChallengeRedeemView(correlationGuid, token));
//    //      if (data == null) return HttpNotFound();
//    //      Guid? userIdByUsername = await Query.Execute(new UserIdByLogin(model.Username));
//    //      Guid? userIdByContact = await Query.Execute(new UserIdByLogin(data.ContactValue));
//    //
//    //      await Command.SendAsync(new RedeemUserRegistrationChallenge(data.UserId, correlationGuid, token,
//    //        data.ContactValue, userIdByContact, model.Username, userIdByUsername, model.Password, model.PasswordConfirmation))
//    //          .ConfigureAwait(false);
//    //
//    //      WebSockets.AddCorrelationService(correlationId);
//    //      var correlationUrl = WebSockets.GetCorrelationUri(correlationId).ToString();
//    //      Response.Headers.Add("X-Correlation-Socket", correlationUrl);
//    //      var location = Url.RouteUrl("LoginRoute");
//    //      return new CreatedResult(location, new { CorrelationId = correlationId, });
//    //    }
//
//    //[HttpPost, Route("api/validate-username")]
//    //public async Task<IActionResult> PostValidateUsername([FromBody] PostCheckUsernameRequest model) {
//    //  await Task.Delay(2000);
//    //  if (model == null) return HttpBadRequest(new object());
//    //  Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(model.Username));
//    //  ValidateUsername validation = new ValidateUsername(model.Username, userIdByLogin);
//    //  CommandRejectionReason? reasonInvalid = validation.Errors
//    //    .SelectMany(x => x.Value.Select(y => y.Reason))
//    //    .Cast<CommandRejectionReason?>().FirstOrDefault();
//    //  return Ok(new PostCheckUsernameResponse {
//    //    ReasonInvalid = reasonInvalid,
//    //  });
//    //}
//  }
//}
