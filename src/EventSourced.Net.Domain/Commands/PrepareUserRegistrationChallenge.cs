using System;
using System.Security.Principal;

namespace EventSourced.Net
{
  public class PrepareUserRegistrationChallenge : ICommand
  {
    public PrepareUserRegistrationChallenge(Guid correlationId, string emailOrPhone, Guid? userIdByEmailOrPhone, IPrincipal principal) {
      emailOrPhone = emailOrPhone?.Trim();
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(emailOrPhone, nameof(emailOrPhone));
        validate.Null(userIdByEmailOrPhone, nameof(emailOrPhone), CommandRejectionReason.AlreadyExists,
          new { emailOrPhone });
        validate.Empty(principal?.Identity?.AuthenticationType, nameof(principal));
      }

      CorrelationId = correlationId;
      EmailOrPhone = emailOrPhone;
    }

    public Guid CorrelationId { get; }
    public string EmailOrPhone { get; }
  }
}
