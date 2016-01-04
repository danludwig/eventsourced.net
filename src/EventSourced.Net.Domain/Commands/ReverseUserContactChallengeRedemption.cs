using System;

namespace EventSourced.Net
{
  public class ReverseUserContactChallengeRedemption : ICommand
  {
    public ReverseUserContactChallengeRedemption(Guid userId, Guid correlationId) {
      UserId = userId;
      CorrelationId = correlationId;
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
  }
}
