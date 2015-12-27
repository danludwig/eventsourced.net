using System.Threading.Tasks;
using SimpleInjector;

namespace EventSourced.Net.Services.Messaging.Commands
{
  public class ImmediatelyConsistentCommandSender : ISendCommand
  {
    private Container Container { get; }

    public ImmediatelyConsistentCommandSender(Container container) {
      Container = container;
    }

    public async Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand {
      var handlerType = typeof(IHandleCommand<>).MakeGenericType(command.GetType());
      dynamic handler = Container.GetInstance(handlerType);
      await handler.HandleAsync((dynamic)command).ConfigureAwait(false);
    }
  }
}
