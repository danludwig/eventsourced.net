using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users.Internal.Documents
{
  public class UserDocument
  {
    public UserDocument() {
      ContactSmsChallenges = new List<UserDocumentContactSmsChallenge>();
      ContactEmailChallenges = new List<UserDocumentContactEmailChallenge>();
      ConfirmedLogins = new List<string>();
    }

    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Id.ToString();
    public Guid Id { get; set; }

    public IList<UserDocumentContactSmsChallenge> ContactSmsChallenges { get; }
    public IList<UserDocumentContactEmailChallenge> ContactEmailChallenges { get; }

    public UserDocumentContactChallenge GetContactChallengeByCorrelationId(Guid correlationId) {
      return ContactChallenges.SingleOrDefault(x => x.CorrelationId == correlationId);
    }

    public void AddContactChallenge(UserDocumentContactChallenge challenge) {
      var emailChallenge = challenge as UserDocumentContactEmailChallenge;
      if (emailChallenge != null) {
        AddContactChallenge(emailChallenge);
        return;
      }
      var smsChallenge = challenge as UserDocumentContactSmsChallenge;
      if (smsChallenge != null) {
        AddContactChallenge(smsChallenge);
        return;
      }
      throw new NotSupportedException($"'{challenge.GetType().Name}' is not a supported {typeof(UserDocumentContactChallenge).Name} yet.");
    }

    public void AddContactChallenge(UserDocumentContactSmsChallenge challenge) {
      ContactSmsChallenges.Add(challenge);
    }

    public void AddContactChallenge(UserDocumentContactEmailChallenge challenge) {
      ContactEmailChallenges.Add(challenge);
    }

    public IReadOnlyCollection<UserDocumentContactChallenge> ContactChallenges =>
      new ReadOnlyCollection<UserDocumentContactChallenge>(ContactEmailChallenges
        .Cast<UserDocumentContactChallenge>().Union(ContactSmsChallenges).ToArray());

    public IList<string> ConfirmedLogins { get; }

    public void AddConfirmedLogin(Guid correlationId) {
      UserDocumentContactChallenge challenge = GetContactChallengeByCorrelationId(correlationId);
      if (challenge == null) throw new InvalidOperationException(
        $"There is no {typeof(UserDocumentContactChallenge).Name} with correlation id '{correlationId}'.");
      ConfirmedLogins.Add(challenge.ContactValue);
    }
  }
}
