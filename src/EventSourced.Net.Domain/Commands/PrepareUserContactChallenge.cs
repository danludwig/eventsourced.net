using System;

namespace EventSourced.Net
{
  public class PrepareUserContactChallenge : ICommand
  {
    public PrepareUserContactChallenge(Guid correlationId, string emailOrPhone, Guid? userIdByEmailOrPhone, bool isAlreadyLoggedIn) {
      emailOrPhone = emailOrPhone?.Trim();
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(emailOrPhone, nameof(emailOrPhone));
        validate.Null(userIdByEmailOrPhone, nameof(emailOrPhone), CommandRejectionReason.AlreadyExists,
          new { emailOrPhone });
        validate.False(isAlreadyLoggedIn, nameof(isAlreadyLoggedIn));
      }

      CorrelationId = correlationId;
      EmailOrPhone = emailOrPhone;
    }

    public Guid CorrelationId { get; }
    public string EmailOrPhone { get; }
  }
}
