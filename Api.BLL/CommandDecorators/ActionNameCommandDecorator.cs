using Api.Common.Commands;
using System;

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
        private readonly ICommandHandler<TCommand, TResult> handler;

        public ActionNameCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler)
        {
            this.handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public TResult Handle(TCommand cmd)
        {
            var result = this.handler.Handle(cmd);
            return result;
        }

       
    }
}
