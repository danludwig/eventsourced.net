using System;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserContactChallengeViewPrepared : IEvent
  {
    public UserContactChallengeViewPrepared(ContactChallengePrepared domainEvent) {
      ChallengeId = domainEvent.ChallengeId;
      UserId = domainEvent.UserId;
      Purpose = domainEvent.Purpose;
    }

    public Guid ChallengeId { get; }
    public Guid UserId { get; }
    public ContactChallengePurpose Purpose { get; }
  }
}