namespace EventSourced.Net
{
  public enum CommandRejectionReason
  {
    Null,
    Empty,
    InvalidFormat,
    NotEqual,
    Unverified,
    MaxAttempts,
    AlreadyApplied,
    NotFound,
    UnexpectedVersion,
    Deleted,
  }
}
