using System;

namespace EventSourced.Net
{
  public class CreateUserPassword : ICommand
  {
    public CreateUserPassword(Guid userId, Guid correlationId, string token, string password, string passwordConfirmation) {
      if (userId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(userId));
      if (correlationId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(correlationId));
      UserId = userId;
      CorrelationId = correlationId;
      Token = token;
      Password = password;
      PasswordConfirmation = passwordConfirmation;
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
    public string Token { get; }
    public string Password { get; }
    public string PasswordConfirmation { get; }
  }
}