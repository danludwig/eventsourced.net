namespace EventSourced.Net
{
  public class CommandRejection
  {
    public CommandRejection(string key, object value, CommandRejectionReason reason)
      : this(key, value, reason, null, null) { }

    public CommandRejection(string key, object value, CommandRejectionReason reason, string message)
      : this(key, value, reason, message, null) { }

    public CommandRejection(string key, object value, CommandRejectionReason reason, object data)
      : this(key, value, reason, null, data) { }

    public CommandRejection(string key, object value, CommandRejectionReason reason, string message, object data) {
      Key = key;
      Value = value;
      Reason = reason;
      Message = message;
      Data = data;
    }

    public string Key { get; }
    public object Value { get; }
    public CommandRejectionReason Reason { get; }
    public string ReasonText => Reason.ToString();
    public string Message { get; }
    public object Data { get; }
  }
}