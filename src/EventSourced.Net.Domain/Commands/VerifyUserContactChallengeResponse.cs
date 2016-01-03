using System;

namespace EventSourced.Net
{
  public class VerifyUserContactChallengeResponse : ICommand
  {
    public VerifyUserContactChallengeResponse(Guid userId, Guid correlationId, string code) {
      code = code?.Trim();
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(userId, nameof(userId));
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(code, nameof(code));
      }

      UserId = userId;
      CorrelationId = correlationId;
      Code = code?.Trim();
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
    public string Code { get; }
  }
}
