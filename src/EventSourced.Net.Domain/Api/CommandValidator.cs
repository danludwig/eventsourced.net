using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace EventSourced.Net
{
  public class CommandValidator : IDisposable
  {
    private bool ThrowIfErrorsOnDispose { get; }
    public IDictionary<string, CommandRejection[]> Errors { get; }

    public CommandValidator(bool throwIfErrorsOnDispose = true) {
      ThrowIfErrorsOnDispose = throwIfErrorsOnDispose;
      Errors = new Dictionary<string, CommandRejection[]>(StringComparer.OrdinalIgnoreCase);
    }

    public void Dispose() {
      if (ThrowIfErrorsOnDispose && Errors.Any()) throw new CommandRejectedException(Errors);
    }

    public void NotNull(object value, string key) {
      if (value == null) AddError(key, null, CommandRejectionReason.Null);
    }

    public void LoggedOff(IIdentity value, string key) {
      if (value?.AuthenticationType != null)
        AddError(key, value, CommandRejectionReason.NotLoggedOff);
    }

    public void Length(string value, string key, int minLength, int maxLength) {
      if (value != null)
        if (value.Length < minLength || value.Length > maxLength)
          AddError(key, value, CommandRejectionReason.InvalidFormat);
    }

    public void NotEmpty(Guid value, string key) {
      if (value == Guid.Empty) AddError(key, value, CommandRejectionReason.Empty);
    }

    public void NotEmpty(string value, string key) {
      if (string.IsNullOrWhiteSpace(value)) AddError(key, value, CommandRejectionReason.Empty);
    }

    public void AreEqual(string value, string key, string otherValue) {
      if (value != otherValue) AddError(key, value, CommandRejectionReason.NotEqual);
    }

    public void IsAvailable(string value, string key, Func<bool> isAvailable) {
      if (!isAvailable()) {
        AddError(key, value, CommandRejectionReason.AlreadyExists);
      }
    }

    public void OnlyCharacters(string value, string key, string allowedCharacters, bool ignoreCase = true) {
      char[] valueCharacters = value?.ToCharArray();
      char[] allowedCharactersArray = allowedCharacters.ToCharArray();
      if (ignoreCase) {
        var allowedCharactersList = new List<char>();
        allowedCharactersList.AddRange(allowedCharactersArray);
        allowedCharactersList.AddRange(allowedCharacters.ToUpper().ToCharArray());
        allowedCharactersArray = allowedCharactersList.Distinct().ToArray();
      }

      if (valueCharacters != null) {
        bool hasDisallowedCharacters = valueCharacters.Any(x => !allowedCharactersArray.Contains(x));
        if (hasDisallowedCharacters) {
          AddError(key, value, CommandRejectionReason.InvalidFormat);
        }
      }
    }

    public bool HasError(string key, CommandRejectionReason reason) {
      return Errors.ContainsKey(key) && Errors[key].Any(x => x.Reason == reason);
    }

    public void AddErrors(IDictionary<string, CommandRejection[]> errors) {
      if (errors != null) {
        foreach (var errorEntry in errors) {
          foreach (var errorItem in errorEntry.Value) {
            AddError(errorEntry.Key, errorItem.Value, errorItem.Reason, errorItem.Data);
          }
        }
      }
    }

    private void AddError(string key, object value, CommandRejectionReason reason, object data = null) {
      key = key?.Trim() ?? "";
      string errorsKey = Errors.Select(x => x.Key).SingleOrDefault(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
      if (errorsKey != null) key = errorsKey;

      if (!Errors.ContainsKey(key))
        Errors[key] = null;

      CommandRejection[] commandRejections = Errors[key] ?? new CommandRejection[0];
      List<CommandRejection> errorsAsList = commandRejections.ToList();
      errorsAsList.Add(new CommandRejection(key, value, reason, data));
      Errors[key] = errorsAsList.ToArray();
    }
  }
}