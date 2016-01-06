using System;

namespace EventSourced.Net
{
  public class ReverseUserRegistrationChallengeRedemption : ICommand
  {
    public ReverseUserRegistrationChallengeRedemption(Guid userId, Guid correlationId, string duplicateContact, string duplicateUsername) {
      UserId = userId;
      CorrelationId = correlationId;
      DuplicateContact = duplicateContact;
      DuplicateUsername = duplicateUsername;
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
    public string DuplicateContact { get; }
    public string DuplicateUsername { get; }
  }
}
