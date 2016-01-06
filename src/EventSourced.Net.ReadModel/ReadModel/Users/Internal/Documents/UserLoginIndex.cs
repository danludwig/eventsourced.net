using System;
using ArangoDB.Client;
using ArangoDB.Client.Common.Newtonsoft.Json;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserLoginIndex
  {
    public string Login { get; set; }

    [JsonProperty("_key")]
    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Login;

    [DocumentProperty(Identifier = IdentifierType.Revision)]
    [JsonProperty("_rev")]
    public string Revision { get; set; }

    public Guid UserId { get; set; }
    public Guid ChallengeCorrelationId { get; set; }
  }
}
