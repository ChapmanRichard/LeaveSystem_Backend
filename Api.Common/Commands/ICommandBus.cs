namespace Api.Common.Commands
{
    public interface ICommandBus
    {
        //
        // void Submit<TCommand>(TCommand command) where TCommand : ICommand;
        TResult Submit<TResult>(ICommand<TResult> command);
    }


}
