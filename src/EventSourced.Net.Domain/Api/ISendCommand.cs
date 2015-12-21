using System.Threading.Tasks;

namespace EventSourced.Net
{
  public interface ISendCommand
  {
    Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
  }
}