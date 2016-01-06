using System;
using System.Threading.Tasks;
using ArangoDB.Client;
using ArangoDB.Client.Common.Newtonsoft.Json;
using ArangoDB.Client.Data;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Events;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users.Internal.Handlers
{
  public class CreateLogin : IHandleEvent<RegistrationChallengeRedeemed>
  {
    private IExecuteQuery Query { get; }
    private IArangoDatabase Db { get; }
    private IPublishEvent EventPublisher { get; }
    private ISendCommand Command { get; }

    public CreateLogin(IExecuteQuery query, IArangoDatabase db, IPublishEvent eventPublisher, ISendCommand command) {
      Query = query;
      Db = db;
      EventPublisher = eventPublisher;
      Command = command;
    }

    public async Task HandleAsync(RegistrationChallengeRedeemed message) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.AggregateId, throwIfNotFound: true));
      UserDocumentContactChallenge challenge = user.GetContactChallengeByCorrelationId(message.CorrelationId);
      bool? isContactConstraintSatisfied = await IsConstraintSatisfied(challenge.ContactValue, message.AggregateId);
      bool? isUsernameConstraintSatisfied = await IsConstraintSatisfied(message.Username, message.AggregateId);
      if (isContactConstraintSatisfied == true && isUsernameConstraintSatisfied == true) return;

      user.AddConfirmedLogin(message.CorrelationId);
      user.AddConfirmedLogin(message.Username);

      UserLoginIndex contactIndex = BuildUserLoginIndex(message, challenge.NormalizedContactValue);
      UserLoginIndex usernameIndex = BuildUserLoginIndex(message, message.NormalizedUsername);
      var transaction = $@"
        function () {{
          var db = require('internal').db;
          db.{nameof(UserLoginIndex)}.save({JsonConvert.SerializeObject(contactIndex)});
          db.{nameof(UserLoginIndex)}.save({JsonConvert.SerializeObject(usernameIndex)});
          var userKey = '{user.Key}';
          var userJson = {JsonConvert.SerializeObject(user)};
          db._query(aqlQuery`FOR u IN {nameof(UserDocument)} FILTER u._key == ${{userKey}} REPLACE u WITH ${{userJson}} IN {nameof(UserDocument)}`);
        }}";
      var transactionData = new TransactionData {
        Collections = new TransactionCollection {
          Write = new[] { nameof(UserLoginIndex), nameof(UserDocument), }
        },
        Action = transaction,
      };

      try {
        await Db.ExecuteTransactionAsync(transactionData);
      } catch (ArangoServerException) {
        isContactConstraintSatisfied = await IsConstraintSatisfied(challenge.ContactValue, message.AggregateId);
        isUsernameConstraintSatisfied = await IsConstraintSatisfied(message.Username, message.AggregateId);

        if (isContactConstraintSatisfied == false || isUsernameConstraintSatisfied == false) {
          string duplicateContact = isContactConstraintSatisfied == false ? challenge.ContactValue : null;
          string duplicateUsername = isUsernameConstraintSatisfied == false ? message.Username : null;
          await Command.SendAsync(new ReverseUserRegistrationChallengeRedemption(message.AggregateId,
            challenge.CorrelationId, duplicateContact, duplicateUsername));
          return;
        }

        if (isContactConstraintSatisfied != true || isUsernameConstraintSatisfied != true)
          throw;
      }

      await EventPublisher.PublishAsync(new ContactChallengeRedemptionConcluded(message.AggregateId, message.CorrelationId));
    }

    private static UserLoginIndex BuildUserLoginIndex(RegistrationChallengeRedeemed message, string login) {
      UserLoginIndex index = new UserLoginIndex {
        Login = login,
        UserId = message.AggregateId,
        ChallengeCorrelationId = message.CorrelationId,
      };
      return index;
    }

    private async Task<bool?> IsConstraintSatisfied(string login, Guid userId) {
      Guid? userIdByLogin = await Query.Execute(new UserIdByLogin(login));
      if (userIdByLogin.HasValue) {
        return userIdByLogin.Value == userId;
      }
      return null;
    }
  }
}
