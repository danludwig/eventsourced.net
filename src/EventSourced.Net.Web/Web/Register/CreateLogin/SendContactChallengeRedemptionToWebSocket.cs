using System.Collections.Generic;
using System.Threading.Tasks;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Events;
using EventSourced.Net.Services.Web.Sockets;
using WebSocketSharp;

namespace EventSourced.Net.Web.Register.CreateLogin
{
  public class SendContactChallengeRedemptionToWebSocket :
    IHandleEvent<RegistrationChallengeRedemptionReversed>,
    IHandleEvent<ContactChallengeRedemptionConcluded>
  {
    private IServeWebSockets WebSockets { get; }

    public SendContactChallengeRedemptionToWebSocket(IServeWebSockets webSockets) {
      WebSockets = webSockets;
    }

    public Task HandleAsync(RegistrationChallengeRedemptionReversed message) {
      if (!WebSockets.IsCorrelationService(message.CorrelationId)) return Task.FromResult(true);

      string emailOrPhone = message.DuplicateContact;
      string username = message.DuplicateUsername;
      IDictionary<string, CommandRejection> errors = new Dictionary<string, CommandRejection>();
      if (!string.IsNullOrWhiteSpace(username))
        errors.Add(nameof(username),
          new CommandRejection(nameof(username), username, CommandRejectionReason.AlreadyExists));
      if (!string.IsNullOrWhiteSpace(emailOrPhone))
        errors.Add(nameof(emailOrPhone),
          new CommandRejection(nameof(emailOrPhone), emailOrPhone, CommandRejectionReason.AlreadyExists));
      using (WebSocket client = WebSockets.CreateCorrelationClient(message.CorrelationId)) {
        client.Send(new {
          errors,
        });
      }
      WebSockets.RemoveCorrelationService(message.CorrelationId);
      return Task.FromResult(true);
    }

    public Task HandleAsync(ContactChallengeRedemptionConcluded message) {
      if (!WebSockets.IsCorrelationService(message.CorrelationId)) return Task.FromResult(true);

      using (WebSocket client = WebSockets.CreateCorrelationClient(message.CorrelationId)) {
        client.Send(new {
          IsComplete = true,
        });
      }
      WebSockets.RemoveCorrelationService(message.CorrelationId);
      return Task.FromResult(true);
    }
  }
}
