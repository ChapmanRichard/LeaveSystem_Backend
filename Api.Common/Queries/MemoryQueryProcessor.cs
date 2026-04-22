using Api.Common.DI;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Api.Common.Queries
{
    public class MemoryQueryProcessor : IQueryProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public MemoryQueryProcessor(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public TResult ProcessSingleQuery<TResult>(ISingleQuery<TResult> query)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var handlerType =
                    typeof(ISingleQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

                dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);


                return handler.Handle((dynamic)query);
            }
        }

        public IList<TResult> Process<TResult>(IQuery<TResult> query)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var handlerType =
                    typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

                dynamic handler = scope.ServiceProvider.GetRequiredService(handlerType);


                return handler.Handle((dynamic)query);
            }
        }
    }
}
