using System;
using System.Collections.Generic;

namespace EventSourced.Net
{
  public class ValidateUsername
  {
    private const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyz0123456789_-.";
    private const int MinLength = 2;
    private const int MaxLength = 12;

    public ValidateUsername(string username, Guid? userIdByLogin) {
      using (var validate = new CommandValidator(throwIfErrorsOnDispose: false)) {
        validate.NotEmpty(username, nameof(username));
        if (!validate.HasError(nameof(username), CommandRejectionReason.Empty)) {

          validate.OnlyCharacters(username, nameof(username), AllowedCharacters);
          if (!validate.HasError(nameof(username), CommandRejectionReason.InvalidFormat)) {
            validate.Length(username, nameof(username), MinLength, MaxLength);
          }
          if (!validate.HasError(nameof(username), CommandRejectionReason.InvalidFormat)) {
            validate.NotPhoneNumber(username, nameof(username));
            if (!validate.HasError(nameof(username), CommandRejectionReason.PhoneNumber)) {
              validate.IsAvailable(username, nameof(username), () => !userIdByLogin.HasValue);
            }
          }
        }
        Errors = validate.Errors;
      }
    }

    public IDictionary<string, CommandRejection[]> Errors { get; }
  }
}
