using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.ReadModel.Users.Internal.Documents;

namespace EventSourced.Net.ReadModel.Users.Internal.Queries
{
  public class UserDocumentById : IQuery<Task<UserDocument>>
  {
    public Guid Id { get; }
    public bool ThrowIfNotFound { get; }

    internal UserDocumentById(Guid id, bool throwIfNotFound = false) {
      Id = id;
      ThrowIfNotFound = throwIfNotFound;
    }
  }

  public class HandleUserDocumentById : IHandleQuery<UserDocumentById, Task<UserDocument>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserDocumentById(IArangoDatabase db) {
      Db = db;
    }

    public Task<UserDocument> Handle(UserDocumentById query) {
      UserDocument user = Db.Query<UserDocument>().SingleOrDefault(x => x.Id == query.Id);
      if (query.ThrowIfNotFound && user == null) throw new InvalidOperationException(
        $"User with id '{query.Id}' does not exist.");
      return Task.FromResult(user);
    }
  }
}
