using System;

namespace EventSourced.Net
{
  public class RedeemUserRegistrationChallenge : ICommand
  {
    public RedeemUserRegistrationChallenge(Guid userId, Guid correlationId, string token,
      string emailOrPhone, Guid? userIdByContact, string username, Guid? userIdByUsername,
      string password, string passwordConfirmation) {

      using (var validate = new CommandValidator()) {
        validate.NotEmpty(userId, nameof(userId));
        validate.NotEmpty(correlationId, nameof(correlationId));
        validate.NotEmpty(token, nameof(token));
        validate.IsAvailable(emailOrPhone, nameof(emailOrPhone), () => !userIdByContact.HasValue);
        validate.AddErrors(new ValidateUsername(username, userIdByUsername).Errors);
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
      Username = username;
      Password = password;
    }

    public Guid UserId { get; }
    public Guid CorrelationId { get; }
    public string Token { get; }
    public string Username { get; }
    public string Password { get; }
  }
}
