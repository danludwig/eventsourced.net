using System;

namespace EventSourced.Net
{
  public class PrepareUserContactChallenge : ICommand
  {
    public PrepareUserContactChallenge(Guid correlationId, string emailOrPhone) {
      emailOrPhone = emailOrPhone?.Trim();
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(emailOrPhone, nameof(emailOrPhone));
      }

      CorrelationId = correlationId;
      EmailOrPhone = emailOrPhone;
    }

    public Guid CorrelationId { get; }
    public string EmailOrPhone { get; }
  }
}
