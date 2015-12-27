namespace EventSourced.Net
{
  public interface IProcessQuery
  {
    TResult Execute<TResult>(IQuery<TResult> query);
  }
}