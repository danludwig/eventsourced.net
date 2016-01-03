namespace EventSourced.Net
{
  public class CommandRejection
  {
    internal CommandRejection(string key, object value, CommandRejectionReason reason, string message = null) {
      Key = key;
      Value = value;
      Reason = reason;
      Message = message;
    }

    public string Key { get; }
    public object Value { get; }
    public CommandRejectionReason Reason { get; }
    public string Message { get; }
  }
}