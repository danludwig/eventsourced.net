using System;

namespace EventSourced.Net
{
  public class VerifyUserContactChallengeResponse : ICommand
  {
    public VerifyUserContactChallengeResponse(Guid userId, Guid correlationId, string code) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      if (correlationId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(correlationId));
      UserId = userId;
      CorrelationId = correlationId;
      Code = code?.Trim();
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
    public string Code { get; }
  }
}