using System.Threading.Tasks;
using System.Transactions;

namespace Api.Common.Commands.Decorators
{
    public class TransactionCommandHandlerDecortator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> handler;
        public TransactionCommandHandlerDecortator(ICommandHandler<TCommand, TResult> handler)
        {
            this.handler = handler;
        }

        public TResult Handle(TCommand command)
        {
            var tr = typeof(TResult);
            if (tr.IsGenericType)
            {
                if (typeof(Task<>) == tr.GetGenericTypeDefinition())
                {
                    var result = this.handler.Handle(command);
                    return result;
                }
            }
            //using (TransactionScope scope = new TransactionScope())
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = this.handler.Handle(command);
                scope.Complete();
                return result;
            }
        }


    }
}
