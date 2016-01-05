using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourced.Net
{
  public class CommandValidator : IDisposable
  {
    public IDictionary<string, CommandRejection[]> Errors { get; }

    public CommandValidator() {
      Errors = new Dictionary<string, CommandRejection[]>(StringComparer.OrdinalIgnoreCase);
    }

    public void Dispose() {
      if (Errors.Any()) throw new CommandRejectedException(Errors);
    }

    public void NotNull(object value, string key) {
      if (value == null) AddError(key, null, CommandRejectionReason.Null);
    }

    public void Null(Guid? value, string key, CommandRejectionReason reason = CommandRejectionReason.NotNull, object data = null) {
      if (value.HasValue) AddError(key, null, reason, data);
    }

    public void False(bool value, string key) {
      if (value) AddError(key, true, CommandRejectionReason.NotFalse);
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

    public bool HasError(string key, CommandRejectionReason reason) {
      return Errors.ContainsKey(key) && Errors[key].Any(x => x.Reason == reason);
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