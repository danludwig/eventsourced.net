using System;

namespace EventSourced.Net.Domain.Users
{
  public abstract class ContactChallengePrepared : IDomainEvent
  {
    protected ContactChallengePrepared(Guid correlationId, Guid challengeId, Guid userId,
      ContactChallengePurpose purpose, string stamp, string contactValue,
      string code, string token) {

      CorrelationId = correlationId;
      ChallengeId = challengeId;
      UserId = userId;
      Purpose = purpose;
      Stamp = stamp;
      Code = code;
      Token = token;
    }

    public Guid CorrelationId { get; }
    public Guid ChallengeId { get; }
    public Guid UserId { get; }
    public ContactChallengePurpose Purpose { get; }
    public string Stamp { get; }
    public string Code { get; }
    public string Token { get; }
  }
}