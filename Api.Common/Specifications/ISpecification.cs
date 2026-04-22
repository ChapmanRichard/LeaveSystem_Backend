using System;
using System.Linq.Expressions;

namespace Api.Common.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        bool IsSatisfiedBy(T entity);
    }
}
