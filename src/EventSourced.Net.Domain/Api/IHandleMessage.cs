using System.Threading.Tasks;

namespace EventSourced.Net
{
  public interface IHandleMessage<in TMessage> where TMessage : IMessage
  {
    Task HandleAsync(TMessage message);
  }
}