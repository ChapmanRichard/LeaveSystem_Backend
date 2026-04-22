using System.Collections.Generic;

namespace Api.Common.Queries
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        IList<TResult> Handle(TQuery query);
    }

    public interface ISingleQueryHandler<TQuery, TResult> where TQuery : ISingleQuery<TResult>
    {
        TResult Handle(TQuery query);
    }
}
