using System;
using System.Threading.Tasks;
using CommonDomain.Persistence;

namespace EventSourced.Net.Domain.Users
{
  public class HandleVerifyUserLogin : IHandleCommand<VerifyUserLogin>
  {
    public HandleVerifyUserLogin(IRepository repository) {
      Repository = repository;
    }

    private IRepository Repository { get; }

    public async Task HandleAsync(VerifyUserLogin message) {
      var user = await Repository.GetByIdAsync<User>(message.UserId);
      Exception exceptionToThrowAfterSave;
      user.VerifyLogin(message.Login, message.Password, out exceptionToThrowAfterSave);

      var commitId = Guid.NewGuid();
      await Repository.SaveAsync(user, commitId);
      if (exceptionToThrowAfterSave != null) throw exceptionToThrowAfterSave;
    }
  }
}