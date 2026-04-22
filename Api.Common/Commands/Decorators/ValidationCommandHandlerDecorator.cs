
using Api.Common.Vaildators;
using System.Collections.Generic;

namespace Api.Common.Commands.Decorators
{
    public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> handler;
        private readonly IEnumerable<IValidator<TCommand>> validators;

        public ValidationCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler,
             IEnumerable<IValidator<TCommand>> validators)
        {
            this.handler = handler;
            this.validators = validators;
        }

        public TResult Handle(TCommand command)
        {
            foreach (var validator in validators)
            {
                validator.ValidateObject(command);
            }
            return this.handler.Handle(command);
        }


    }
}
