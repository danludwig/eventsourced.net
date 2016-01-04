using System;
using System.Linq;
using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.ReadModel.Users.Internal.Documents;

namespace EventSourced.Net.ReadModel.Users
{
  public class UserIdByLogin : IQuery<Task<Guid?>>
  {
    public string Login { get; }

    public UserIdByLogin(string login) {
      Login = login;
    }
  }

  public class HandleUserIdByLogin : IHandleQuery<UserIdByLogin, Task<Guid?>>
  {
    private IArangoDatabase Db { get; }

    public HandleUserIdByLogin(IArangoDatabase db) {
      Db = db;
    }

    public Task<Guid?> Handle(UserIdByLogin query) {
      Guid? userId = null;
      string normalizedLogin = ContactIdParser.Normalize(query.Login);
      UserLoginIndex index = Db.Query<UserLoginIndex>().SingleOrDefault(x => x.Key == normalizedLogin);
      if (index != null) userId = index.UserId;
      return Task.FromResult(userId);
    }
  }
}
