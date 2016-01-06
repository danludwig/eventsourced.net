namespace EventSourced.Net
{
  public enum CommandRejectionReason
  {
    Unknown,
    Null,
    Empty,
    NotLoggedOff,
    InvalidFormat,
    NotEqual,
    Unverified,
    MaxAttempts,
    AlreadyApplied,
    AlreadyExists,
    PhoneNumber,
    NotFound,
    UnexpectedVersion,
    Deleted,
  }
}
