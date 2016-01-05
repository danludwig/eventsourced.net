namespace EventSourced.Net
{
  public enum CommandRejectionReason
  {
    Null,
    NotNull,
    NotFalse,
    Empty,
    NotEmpty,
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
