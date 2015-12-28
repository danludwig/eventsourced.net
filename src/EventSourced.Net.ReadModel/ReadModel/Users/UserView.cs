using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserView
  {
    public UserView() {
      ContactSmsChallenges = new List<UserContactSmsChallengeView>();
      ContactEmailChallenges = new List<UserContactEmailChallengeView>();
    }

    [DocumentProperty(Identifier = IdentifierType.Key)]
    public string Key => Id.ToString();
    public Guid Id { get; set; }

    public IList<UserContactSmsChallengeView> ContactSmsChallenges { get; }
    public IList<UserContactEmailChallengeView> ContactEmailChallenges { get; }

    public UserContactChallengeView ContactChallengeById(Guid challengeId) {
      return ContactChallenges.SingleOrDefault(x => x.Id == challengeId);
    }

    public void AddContactChallenge(UserContactChallengeView challenge) {
      UserContactEmailChallengeView emailChallenge = challenge as UserContactEmailChallengeView;
      if (emailChallenge != null) {
        AddContactChallenge(emailChallenge);
        return;
      }
      UserContactSmsChallengeView smsChallenge = challenge as UserContactSmsChallengeView;
      if (smsChallenge != null) {
        AddContactChallenge(smsChallenge);
        return;
      }
      throw new NotSupportedException($"'{challenge.GetType().Name}' is not a supported {typeof(UserContactChallengeView).Name} yet.");
    }

    public void AddContactChallenge(UserContactSmsChallengeView challenge) {
      ContactSmsChallenges.Add(challenge);
    }

    public void AddContactChallenge(UserContactEmailChallengeView challenge) {
      ContactEmailChallenges.Add(challenge);
    }

    public IReadOnlyCollection<UserContactChallengeView> ContactChallenges =>
      new ReadOnlyCollection<UserContactChallengeView>(ContactEmailChallenges
        .Cast<UserContactChallengeView>().Union(ContactSmsChallenges).ToArray());
  }
}