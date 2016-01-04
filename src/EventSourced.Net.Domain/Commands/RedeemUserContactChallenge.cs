using System;

namespace EventSourced.Net
{
  public class RedeemUserContactChallenge : ICommand
  {
    public RedeemUserContactChallenge(Guid userId, Guid correlationId, string token, string password, string passwordConfirmation) {
      using (var validate = new CommandValidator()) {
        validate.NotEmpty(userId, nameof(userId));
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(token, nameof(token));
        validate.NotEmpty(password, nameof(password));
        validate.NotEmpty(passwordConfirmation, nameof(passwordConfirmation));
        if (!validate.HasError(nameof(password), CommandRejectionReason.Empty)
          && !validate.HasError(nameof(passwordConfirmation), CommandRejectionReason.Empty)) {
          validate.AreEqual(password, nameof(password), passwordConfirmation);
        }
      }

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
