using System;
using System.Threading.Tasks;
using CommonDomain;
using CommonDomain.Persistence;

namespace EventSourced.Net
{
  public static class RepositoryExtensions
  {
    public static async Task<RepositoryGetResult<TAggregate>> TryGetByIdAsync<TAggregate>(this IRepository repository, Guid id)
      where TAggregate : class, IAggregate {
      try {
        TAggregate aggregate = await repository.GetByIdAsync<TAggregate>(id);
        return new RepositoryGetResult<TAggregate>(RepositoryGetStatus.Ok, aggregate);
      } catch (AggregateNotFoundException) {
        return new RepositoryGetResult<TAggregate>(RepositoryGetStatus.NotFound);
      } catch (AggregateDeletedException) {
        return new RepositoryGetResult<TAggregate>(RepositoryGetStatus.Deleted);
      } catch (AggregateVersionException) {
        return new RepositoryGetResult<TAggregate>(RepositoryGetStatus.UnexpectedVersion);
      }
    }

    public static void RejectIfNull<TAggregate>(this RepositoryGetResult<TAggregate> result, string key, Guid id)
      where TAggregate : class, IAggregate {
      if (result.Aggregate == null) {
        throw new CommandRejectedException(key, id, CommandRejectionReason.Null);
      }
    }
  }
}
