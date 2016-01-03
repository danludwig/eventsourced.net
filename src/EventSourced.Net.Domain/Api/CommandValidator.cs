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

    private void AddError(string key, object value, CommandRejectionReason reason) {
      key = key?.Trim() ?? "";
      string errorsKey = Errors.Select(x => x.Key).SingleOrDefault(x => x.Equals(key, StringComparison.InvariantCultureIgnoreCase));
      if (errorsKey != null) key = errorsKey;

      if (!Errors.ContainsKey(key))
        Errors[key] = null;

      CommandRejection[] commandRejections = Errors[key] ?? new CommandRejection[0];
      List<CommandRejection> errorsAsList = commandRejections.ToList();
      errorsAsList.Add(new CommandRejection(key, value, reason));
      Errors[key] = errorsAsList.ToArray();
    }
  }
}