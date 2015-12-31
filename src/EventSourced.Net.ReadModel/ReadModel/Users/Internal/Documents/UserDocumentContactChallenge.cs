using System;
using ArangoDB.Client.Common.Newtonsoft.Json;
using ArangoDB.Client.Common.Newtonsoft.Json.Converters;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public abstract class UserDocumentContactChallenge
  {
    protected UserDocumentContactChallenge() { }

    protected UserDocumentContactChallenge(ContactChallengePrepared message) {
      CorrelationId = message.CorrelationId;
      Token = message.Token;
      Purpose = message.Purpose;
    }

    public Guid CorrelationId { get; set; }
    public string Token { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public ContactChallengePurpose Purpose { get; set; }
    [ArangoDB.Client.Common.Newtonsoft.Json.JsonIgnore]
    public abstract string ContactValue { get; }
  }
}