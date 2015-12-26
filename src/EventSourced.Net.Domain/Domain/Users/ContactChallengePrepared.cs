using System;
using EventSourced.Net.Domain.Users.ContactChallengers;

namespace EventSourced.Net.Domain.Users
{
  public abstract class ContactChallengePrepared : IDomainEvent
  {
    protected ContactChallengePrepared(Guid challengeId, Guid userId,
      ContactChallengePurpose purpose, string stamp, string contactValue,
      string code, string token) {

      ChallengeId = challengeId;
      UserId = userId;
      Purpose = purpose;
      Stamp = stamp;
      Code = code ?? TotpCodeProvider.Generate(userId, contactValue, purpose, stamp);
      Token = token ?? DataProtectionTokenProvider.Instance.Generate(userId, purpose, stamp);
    }

    public Guid ChallengeId { get; }
    public Guid UserId { get; }
    public ContactChallengePurpose Purpose { get; }
    public string Stamp { get; }
    public string Code { get; }
    public string Token { get; }
  }
}