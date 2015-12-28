using System;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public abstract class UserContactChallengeView
  {
    protected UserContactChallengeView() { }

    protected UserContactChallengeView(ContactChallengePrepared message) {
      Id = message.ChallengeId;
      UserId = message.UserId;
      CorrelationId = message.CorrelationId;
      Token = message.Token;
      PurposeEnum = message.Purpose;
    }

    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Id.ToString();
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid CorrelationId { get; set; }
    public string Token { get; set; }
    public ContactChallengePurpose PurposeEnum { get; set; }
    public string PurposeText => PurposeEnum.ToString();
  }
}