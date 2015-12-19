using System;
using System.Threading.Tasks;

namespace CommonDomain.Persistence
{
  public static class RepositoryExtensions
  {
    public static async Task SaveAsync(this IRepository repository, IAggregate aggregate, Guid commitId) {
      await repository.SaveAsync(aggregate, commitId, a => { });
    }
  }
}