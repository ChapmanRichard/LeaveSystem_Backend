namespace Api.Common.Commands
{


    //public interface ICommandHandler<TCommand>
    //{
    //    void Handle(TCommand command);
    //}

    // For simple solution and development tradeoff, command can return 
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        TResult Handle(TCommand cmd);
    }

}
