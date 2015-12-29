using System;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public abstract class UserDocumentContactChallenge
  {
    protected UserDocumentContactChallenge() { }

    protected UserDocumentContactChallenge(ContactChallengePrepared message) {
      CorrelationId = message.CorrelationId;
      Token = message.Token;
      PurposeEnum = message.Purpose;
    }

    public Guid CorrelationId { get; set; }
    public string Token { get; set; }
    public ContactChallengePurpose PurposeEnum { get; set; }
    public string PurposeText => PurposeEnum.ToString();
    public abstract string ContactValue { get; }
  }
}