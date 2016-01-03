namespace EventSourced.Net
{
  public enum CommandRejectionReason
  {
    Null,
    Empty,
    InvalidFormat,
    StateConflict,
    Unverified,
    MaxAttempts,
  }
}