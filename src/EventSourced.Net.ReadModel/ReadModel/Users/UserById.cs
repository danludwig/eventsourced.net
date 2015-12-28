using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserById : IQuery<Task<UserView>>
  {
    public Guid Id { get; }

    public UserById(Guid id) {
      Id = id;
    }
  }

  public class HandleUserById : IHandleQuery<UserById, Task<UserView>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserById(IArangoDatabase db) {
      Db = db;
    }

    public Task<UserView> Handle(UserById query) {
      var user = Db.Query<UserView>().SingleOrDefault(x => x.Id == query.Id);
      return Task.FromResult(user);
    }
  }
}
