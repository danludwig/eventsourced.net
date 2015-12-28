using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class HandleCreateUserPassword : IHandleCommand<CreateUserPassword>
  {
    public HandleCreateUserPassword(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(CreateUserPassword message) {
      var user = await Repository.GetByIdAsync<User>(message.UserId);
      user.CreatePassword(message.CorrelationId, message.Token, message.Password, message.PasswordConfirmation);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
    }
  }
}
