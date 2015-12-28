using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;

namespace EventSourced.Net.ReadModel.Users
{
  public class HandlePasswordCreated : IHandleEvent<PasswordCreated>
  {
    private IProcessQuery Query { get; }
    private IArangoDatabase Db { get; }

    public HandlePasswordCreated(IProcessQuery query, IArangoDatabase db) {
      Query = query;
      Db = db;
    }

    public async Task HandleAsync(PasswordCreated message) {
      UserView user = await Query.Execute(new UserById(message.UserId));
      user.AddConfirmedLogin(message.CorrelationId);
      await Db.UpdateAsync<UserView>(user);
    }
  }
}
