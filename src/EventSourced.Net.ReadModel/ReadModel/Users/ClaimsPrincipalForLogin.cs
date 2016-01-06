using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users
{
  public class ClaimsByUserId : IQuery<Task<Claim[]>>
  {
    public Guid UserId { get; }

    public ClaimsByUserId(Guid userId) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      UserId = userId;
    }
  }

  public class HandleClaimsByUserId : IHandleQuery<ClaimsByUserId, Task<Claim[]>>
  {
    private IExecuteQuery Query { get; }

    public HandleClaimsByUserId(IExecuteQuery query) {
      Query = query;
    }

    public async Task<Claim[]> Handle(ClaimsByUserId query) {
      IList<Claim> claims = new List<Claim>();
      claims.Add(new Claim(ClaimTypes.NameIdentifier, query.UserId.ToString()));

      var user = await Query.Execute(new UserDocumentById(query.UserId));
      if (user != null) {
        if (!string.IsNullOrWhiteSpace(user.Username)) {
          claims.Add(new Claim(ClaimTypes.Name, user.Username));
        }

        foreach (var normalizedLogin in user.ConfirmedLogins.Select(ContactIdParser.Normalize)) {
          if (normalizedLogin == ContactIdParser.Normalize(user.Username)) continue;
          var matchingChallenge = user.ContactChallenges.FirstOrDefault(x =>
            string.Equals(ContactIdParser.Normalize(x.ContactValue), normalizedLogin));
          if (matchingChallenge == null) continue;
          switch (matchingChallenge.Purpose) {
            case ContactChallengePurpose.CreateUserFromEmail:
              claims.Add(new Claim(ClaimTypes.Email, matchingChallenge.ContactValue));
              break;
            case ContactChallengePurpose.CreateUserFromPhone:
              claims.Add(new Claim(ClaimTypes.MobilePhone, matchingChallenge.ContactValue));
              break;
          }
        }
      }

      return claims.ToArray();
    }
  }
}
