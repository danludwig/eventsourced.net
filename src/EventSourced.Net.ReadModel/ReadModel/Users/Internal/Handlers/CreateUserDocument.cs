using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users.Internal.Handlers
{
  public class CreateUserDocument : IHandleEvent<UserCreated>
  {
    private IExecuteQuery Query { get; }
    private IArangoDatabase Db { get; }

    public CreateUserDocument(IExecuteQuery query, IArangoDatabase db) {
      Query = query;
      Db = db;
    }

    public async Task HandleAsync(UserCreated message) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.AggregateId));
      if (user == null) {
        user = new UserDocument { Id = message.AggregateId, };
        await Db.InsertAsync<UserDocument>(user);
      }
    }
  }
}
