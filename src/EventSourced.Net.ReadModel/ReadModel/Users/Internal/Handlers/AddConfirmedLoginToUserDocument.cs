using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users.Internal.Handlers
{
  public class AddConfirmedLoginToUserDocument : IHandleEvent<PasswordCreated>
  {
    private IExecuteQuery Query { get; }
    private IArangoDatabase Db { get; }

    public AddConfirmedLoginToUserDocument(IExecuteQuery query, IArangoDatabase db) {
      Query = query;
      Db = db;
    }

    public async Task HandleAsync(PasswordCreated message) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.AggregateId, throwIfNotFound: true));
      user.AddConfirmedLogin(message.CorrelationId);
      await Db.UpdateAsync<UserDocument>(user);
    }
  }
}
