using CommonDomain;

namespace EventSourced.Net
{
  public class RepositoryGetResult<TAggregate> where TAggregate : class, IAggregate
  {
    public RepositoryGetResult(RepositoryGetStatus status, TAggregate aggregate = null) {
      Status = status;
      Aggregate = aggregate;
    }

    public RepositoryGetStatus Status { get; }
    public TAggregate Aggregate { get; }
  }
}