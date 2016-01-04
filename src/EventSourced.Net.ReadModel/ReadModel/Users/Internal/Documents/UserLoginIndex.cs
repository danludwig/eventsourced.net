using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArangoDB.Client;
using ArangoDB.Client.Common.Newtonsoft.Json;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserLoginIndex
  {
    public UserLoginIndex() {
    }

    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Login;
    public string Login { get; set; }
    public Guid UserId { get; set; }
    public Guid ChallengeCorrelationId { get; set; }
  }
}
