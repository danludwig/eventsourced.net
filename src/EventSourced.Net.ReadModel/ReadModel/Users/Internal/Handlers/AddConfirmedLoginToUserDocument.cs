using System;
using System.Threading.Tasks;
using ArangoDB.Client;
using EventSourced.Net.Domain.Users;
using EventSourced.Net.ReadModel.Users.Internal.Documents;
using EventSourced.Net.ReadModel.Users.Internal.Events;
using EventSourced.Net.ReadModel.Users.Internal.Queries;

namespace EventSourced.Net.ReadModel.Users.Internal.Handlers
{
  public class AddConfirmedLoginToUserDocument : IHandleEvent<ContactChallengeRedeemed>
  {
    private IExecuteQuery Query { get; }
    private IArangoDatabase Db { get; }
    private IPublishEvent EventPublisher { get; }
    private ISendCommand Command { get; }

    public AddConfirmedLoginToUserDocument(IExecuteQuery query, IArangoDatabase db, IPublishEvent eventPublisher, ISendCommand command) {
      Query = query;
      Db = db;
      EventPublisher = eventPublisher;
      Command = command;
    }

    public async Task HandleAsync(ContactChallengeRedeemed message) {
      UserDocument user = await Query.Execute(new UserDocumentById(message.AggregateId, throwIfNotFound: true));
      UserDocumentContactChallenge challenge = user.GetContactChallengeByCorrelationId(message.CorrelationId);
      string login = challenge.ContactValue;

      var isConstraintSatisfied = await IsConstraintSatisfied(message.AggregateId, challenge);
      if (isConstraintSatisfied.HasValue && !isConstraintSatisfied.Value) {
        return;
      }

      if (!isConstraintSatisfied.HasValue) {
        try {
          UserLoginIndex index = new UserLoginIndex {
            Login = login,
            UserId = message.AggregateId,
            ChallengeCorrelationId = message.CorrelationId
          };
          await Db.InsertAsync<UserLoginIndex>(index);
        } catch (ArangoServerException) {
          isConstraintSatisfied = await IsConstraintSatisfied(message.AggregateId, challenge);
          if (isConstraintSatisfied.HasValue && !isConstraintSatisfied.Value) {
            return;
          }
          throw;
        }
      }

      user.AddConfirmedLogin(message.CorrelationId);
      await Db.UpdateAsync<UserDocument>(user);
      await EventPublisher.PublishAsync(new ContactChallengeRedemptionConcluded(message.AggregateId, message.CorrelationId));
    }

    private async Task<bool?> IsConstraintSatisfied(Guid userId, UserDocumentContactChallenge challenge) {
      Guid? loginUserId = await Query.Execute(new UserIdByLogin(challenge.ContactValue));
      if (!loginUserId.HasValue) return null;
      if (loginUserId.Value == userId) return true;

      await Command.SendAsync(new ReverseUserContactChallengeRedemption(userId, challenge.CorrelationId));
      return false;
    }
  }
}
