using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserDocument
  {
    public UserDocument() {
      ContactSmsChallenges = new List<UserContactSmsChallenge>();
      ContactEmailChallenges = new List<UserContactEmailChallenge>();
    }

    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Id.ToString();
    public Guid Id { get; set; }

    public IList<UserContactSmsChallenge> ContactSmsChallenges { get; }
    public IList<UserContactEmailChallenge> ContactEmailChallenges { get; }

    public UserContactChallenge ContactChallengeById(Guid challengeId) {
      return ContactChallenges.SingleOrDefault(x => x.Id == challengeId);
    }

    public void AddContactChallenge(UserContactChallenge challenge) {
      UserContactEmailChallenge emailChallenge = challenge as UserContactEmailChallenge;
      if (emailChallenge != null) {
        AddContactChallenge(emailChallenge);
        return;
      }
      UserContactSmsChallenge smsChallenge = challenge as UserContactSmsChallenge;
      if (smsChallenge != null) {
        AddContactChallenge(smsChallenge);
        return;
      }
      throw new NotSupportedException($"'{challenge.GetType().Name}' is not a supported {typeof(UserContactChallenge).Name} yet.");
    }

    public void AddContactChallenge(UserContactSmsChallenge challenge) {
      ContactSmsChallenges.Add(challenge);
    }

    public void AddContactChallenge(UserContactEmailChallenge challenge) {
      ContactEmailChallenges.Add(challenge);
    }

    private IReadOnlyCollection<UserContactChallenge> ContactChallenges =>
      new ReadOnlyCollection<UserContactChallenge>(ContactEmailChallenges
        .Cast<UserContactChallenge>().Union(ContactSmsChallenges).ToArray());
  }
}