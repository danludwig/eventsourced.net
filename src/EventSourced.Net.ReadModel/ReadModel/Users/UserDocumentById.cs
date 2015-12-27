using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserDocumentById : IQuery<Task<UserDocument>>
  {
    public Guid Id { get; }

    public UserDocumentById(Guid id) {
      Id = id;
    }
  }

  public class HandleUserDocumentById : IHandleQuery<UserDocumentById, Task<UserDocument>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserDocumentById(IArangoDatabase db) {
      Db = db;
    }

    public Task<UserDocument> Handle(UserDocumentById query) {
      var user = Db.Query<UserDocument>().SingleOrDefault(x => x.Id == query.Id);
      return Task.FromResult(user);
    }
  }
}
