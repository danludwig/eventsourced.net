using System;
using System.Collections.Generic;

namespace EventSourced.Net
{
  public class CommandRejectedException : Exception
  {
    private const string BaseMessage = "The command has been rejected because it contains invalid input.";
    public IDictionary<string, CommandRejection[]> Errors { get; }

    internal CommandRejectedException(IDictionary<string, CommandRejection[]> errors) : base(BaseMessage) {
      Errors = errors;
    }

    internal CommandRejectedException(string key, object value, CommandRejectionReason reason, string message = null)
      : base(BaseMessage) {
      Errors = new Dictionary<string, CommandRejection[]>(StringComparer.OrdinalIgnoreCase) {
        { key, new[] { new CommandRejection(key, value, reason, message), } }
      };
    }
  }
}
