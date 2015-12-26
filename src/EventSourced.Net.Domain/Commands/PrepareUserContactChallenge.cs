using System;

namespace EventSourced.Net
{
  public class PrepareUserContactChallenge : ICommand
  {
    public PrepareUserContactChallenge(Guid correlationId, string emailOrPhone) {
      if (correlationId == Guid.Empty) throw new ArgumentException("Cannot be empty.", nameof(correlationId));
      CorrelationId = correlationId;
      EmailOrPhone = emailOrPhone?.Trim();
    }

    public Guid CorrelationId { get; }
    public string EmailOrPhone { get; }
  }
}
