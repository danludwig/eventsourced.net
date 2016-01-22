﻿using System;
using System.Linq;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users;
using EventSourced.Net.Services.Web.Mvc;
using EventSourced.Net.Services.Web.Sockets;
using EventSourced.Net.Web.Home;
using Microsoft.AspNet.Mvc;

namespace EventSourced.Net.Web.Users.Register
{
  [Return400BadRequstIfCommandRejected]
  public class ApiController : Controller
  {
    private ISendCommand Command { get; }
    private IExecuteQuery Query { get; }
    private IServeWebSockets WebSockets { get; }

    public ApiController(ISendCommand command, IExecuteQuery query, IServeWebSockets webSockets) {
      Command = command;
      Query = query;
      WebSockets = webSockets;
    }

    [HttpGet, Route("api/register/{correlationId}/redeem")]
    public async Task<IActionResult> GetInitialState(string correlationId, string token) {
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();

      UserRegistrationRedeemData data = await Query
        .Execute(new UserRegistrationRedeemView(correlationGuid, token));
      if (data == null) return HttpNotFound();
      var payload = new ReduxStateResponse(User);
      payload.Ui.Redeem = new RedeemPayload {
        Data = new VerifyResponseModel {
          ContactValue = data.ContactValue,
          Purpose = data.Purpose,
        },
      };
      return Ok(payload);
    }

    [HttpPost, Route("api/register")]
    public async Task<IActionResult> PostChallenge([FromBody] RegisterRequestModel model) {
      if (model == null) return HttpBadRequest();
      //await Task.Delay(1000);
      ShortGuid correlationId = Guid.NewGuid();
      Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(model.EmailOrPhone));

      await Command.SendAsync(new PrepareUserRegistrationChallenge(correlationId, model.EmailOrPhone,
        userIdByLogin, User)).ConfigureAwait(false);

      string location = Url.RouteUrl("RegisterVerifyRoute", new { correlationId });
      return new CreatedResult(location, new { correlationId, });
    }

    [HttpPost, Route("api/register/{correlationId}")]
    public async Task<IActionResult> PostVerify(string correlationId, [FromBody] VerifyRequestModel model) {
      //await Task.Delay(1000);
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();
      UserContactChallengeTokenData data = await Query.Execute(new UserContactChallengeTokenView(correlationGuid));
      if (data == null) return HttpNotFound();

      await Command.SendAsync(new VerifyUserContactChallengeResponse(data.UserId, correlationGuid, model.Code))
        .ConfigureAwait(false);

      var location = Url.RouteUrl("RegisterRedeemRoute", new { token = data.Token, });
      return new CreatedResult(location, new VerifyResponseModel {
        ContactValue = data.ContactValue,
        Purpose = data.Purpose,
      });
    }

    [HttpPost, Route("api/register/{correlationId}/redeem")]
    public async Task<IActionResult> PostRedeem(string correlationId, string token, string username, string password, string passwordConfirmation) {
      Guid correlationGuid;
      if (!ShortGuid.TryParseGuid(correlationId, out correlationGuid)) return HttpNotFound();

      UserRegistrationRedeemData data = await Query
        .Execute(new UserRegistrationRedeemView(correlationGuid, token));
      if (data == null) return HttpNotFound();
      Guid? userIdByUsername = await Query.Execute(new UserIdByLogin(username));
      Guid? userIdByContact = await Query.Execute(new UserIdByLogin(data.ContactValue));

      await Command.SendAsync(new RedeemUserRegistrationChallenge(data.UserId, correlationGuid, token,
        data.ContactValue, userIdByContact, username, userIdByUsername, password, passwordConfirmation))
          .ConfigureAwait(false);

      WebSockets.AddCorrelationService(correlationId);
      var correlationUrl = WebSockets.GetCorrelationUri(correlationId).ToString();
      Response.Headers.Add("X-Correlation-Socket", correlationUrl);
      var location = Url.RouteUrl("LoginRoute");
      return new CreatedResult(location, new { CorrelationId = correlationId, });
    }

    [HttpPost, Route("api/check-username", Name = "CheckUsernameRoute")]
    public async Task<IActionResult> PostCheckUsername(string username) {
      Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(username));
      ValidateUsername validation = new ValidateUsername(username, userIdByLogin);
      CommandRejectionReason? reasonInvalid = validation.Errors
        .SelectMany(x => x.Value.Select(y => y.Reason))
        .Cast<CommandRejectionReason?>().FirstOrDefault();
      return Ok(new UsernameCheckResponseModel {
        ReasonInvalid = reasonInvalid,
      });
    }
  }
}
