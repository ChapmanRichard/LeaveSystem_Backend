using Api.Common.Vaildators;
using System.Collections.Generic;

namespace Api.Common.Queries.Decorators
{
    public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private readonly IEnumerable<IValidator<TQuery>> validators;

        public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> decorated, IEnumerable<IValidator<TQuery>> validators)
        {
            this.decorated = decorated;
            this.validators = validators;
        }

        public IList<TResult> Handle(TQuery query)
        {
            foreach (var validator in validators)
            {
                validator.ValidateObject(query);
            }
            return this.decorated.Handle(query);
        }
    }

    public class ValidationSingleQueryHandlerDecorator<TQuery, TResult> : ISingleQueryHandler<TQuery, TResult> where TQuery : ISingleQuery<TResult>
    {
        private readonly ISingleQueryHandler<TQuery, TResult> decorated;
        private readonly IEnumerable<IValidator<TQuery>> validators;

        public ValidationSingleQueryHandlerDecorator(ISingleQueryHandler<TQuery, TResult> decorated, IEnumerable<IValidator<TQuery>> validators)
        {
            this.decorated = decorated;
            this.validators = validators;
        }

        public TResult Handle(TQuery query)
        {
            foreach (var validator in validators)
            {
                validator.ValidateObject(query);
            }
            return this.decorated.Handle(query);
        }
    }
}
