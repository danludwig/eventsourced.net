namespace EventSourced.Net
{
  public enum CommandRejectionReason
  {
    Null,
    Empty,
    NotLoggedOff,
    InvalidFormat,
    NotEqual,
    Unverified,
    MaxAttempts,
    AlreadyApplied,
    AlreadyExists,
    NotFound,
    UnexpectedVersion,
    Deleted,
  }
}
