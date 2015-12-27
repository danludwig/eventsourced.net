using System;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public abstract class UserContactChallenge
  {
    protected UserContactChallenge() { }

    protected UserContactChallenge(ContactChallengePrepared message) {
      Id = message.ChallengeId;
      PurposeEnum = message.Purpose;
    }

    public Guid Id { get; set; }
    public ContactChallengePurpose PurposeEnum { get; set; }
    public string PurposeText => PurposeEnum.ToString();
  }
}