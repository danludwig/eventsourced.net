namespace EventSourced.Net
{
  public interface IHandleQuery<in TQuery, out TResult> where TQuery : IQuery<TResult>
  {
    TResult Handle(TQuery query);
  }
}
