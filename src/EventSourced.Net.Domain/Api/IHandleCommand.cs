namespace EventSourced.Net
{
  public interface IHandleCommand<in TCommand> : IHandleMessage<TCommand> where TCommand : ICommand { }
}