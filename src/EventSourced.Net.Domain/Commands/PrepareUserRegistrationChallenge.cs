using System;
using System.Security.Principal;

namespace EventSourced.Net
{
  public class PrepareUserRegistrationChallenge : ICommand
  {
    public PrepareUserRegistrationChallenge(Guid correlationId, string emailOrPhone, Guid? userIdByEmailOrPhone, IPrincipal principal) {
      emailOrPhone = emailOrPhone?.Trim();
      using (var validate = new CommandValidator()) {
        validate.LoggedOff(principal?.Identity, nameof(principal));
        if (!validate.HasError(nameof(principal), CommandRejectionReason.NotLoggedOff)) {
          validate.NotEmpty(correlationId, nameof(correlationId));
          validate.NotEmpty(emailOrPhone, nameof(emailOrPhone));
          validate.EmailOrPhone(emailOrPhone, nameof(emailOrPhone));
          if (!validate.HasError(nameof(emailOrPhone), CommandRejectionReason.InvalidFormat))
            validate.IsAvailable(emailOrPhone, nameof(emailOrPhone), () => !userIdByEmailOrPhone.HasValue);
        }
      }

      CorrelationId = correlationId;
      EmailOrPhone = emailOrPhone;
      bool isPhone = ContactIdParser.AsPhoneNumber(emailOrPhone) != null;
      bool isEmail = !isPhone && ContactIdParser.AsMailAddress(emailOrPhone) != null;
      if (isEmail) Purpose = ContactChallengePurpose.CreateUserFromEmail;
      else if (isPhone) Purpose = ContactChallengePurpose.CreateUserFromPhone;
      else throw new CommandRejectedException(nameof(emailOrPhone), emailOrPhone, CommandRejectionReason.InvalidFormat);
    }

    public Guid CorrelationId { get; }
    public string EmailOrPhone { get; }
    public ContactChallengePurpose Purpose { get; }
  }
}
