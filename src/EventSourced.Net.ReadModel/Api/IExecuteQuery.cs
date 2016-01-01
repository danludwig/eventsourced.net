namespace EventSourced.Net
{
  public interface IExecuteQuery
  {
    TResult Execute<TResult>(IQuery<TResult> query);
  }
}