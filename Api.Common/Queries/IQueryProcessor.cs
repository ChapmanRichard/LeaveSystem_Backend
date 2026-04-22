using System.Collections.Generic;

namespace Api.Common.Queries
{
    public interface IQueryProcessor
    {
        IList<TResult> Process<TResult>(IQuery<TResult> query);
        TResult ProcessSingleQuery<TResult>(ISingleQuery<TResult> query);
    }
}
