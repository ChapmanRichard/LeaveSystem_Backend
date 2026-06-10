using Api.Common.Commands;

namespace Api.BLL.CommandDecorators
{
    public class ActionNameProvider
    {
        public string ActionName { get; set; } = "";

        public ActionNameProvider()
        {
        }

    }

    public class ActionNameCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {

        private ActionNameProvider actionNameProvider;

        private readonly ICommandHandler<TCommand, TResult> handler;
        public ActionNameCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler, ActionNameProvider actionNameProvider)
        {
            this.handler = handler;
            this.actionNameProvider = actionNameProvider;
        }

        public TResult Handle(TCommand cmd)
        {
            var result = this.handler.Handle(cmd);
            return result;
        }

       
    }
}
