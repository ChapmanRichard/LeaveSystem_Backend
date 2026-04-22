using LinqKit;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Common.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>> _predicate;


        public Specification(Expression<Func<T, bool>> predicate)
        {
            _predicate = predicate;
        }

        public Expression<Func<T, bool>> Predicate
        {
            get { return _predicate; }
        }

        public bool IsSatisfiedBy(T entity)
        {
            return _predicate.Compile().Invoke(entity);
        }

        public static Specification<T> operator &(Specification<T> leftSide, Specification<T> rightSide)
        {

            //var invokedExpr = Expression.Invoke(rightSide.Predicate, leftSide.Predicate.Parameters.Cast<Expression>());
            //return new Specification<T>(Expression.Lambda<Func<T, bool>>(Expression.AnDataAccessso(leftSide.Predicate.Body, invokedExpr), leftSide.Predicate.Parameters));

            var rightInvoke = Expression.Invoke(rightSide.Predicate, leftSide.Predicate.Parameters.Cast<Expression>());
            var newExpression = Expression.MakeBinary(ExpressionType.AndAlso, leftSide.Predicate.Body, rightInvoke);
            return new Specification<T>(
                                    Expression.Lambda<Func<T, bool>>(newExpression, leftSide.Predicate.Parameters).Expand()
                                  );

        }

        public static Specification<T> operator |(Specification<T> leftSide, Specification<T> rightSide)
        {

            var rightInvoke = Expression.Invoke(rightSide.Predicate, leftSide.Predicate.Parameters.Cast<Expression>());
            var newExpression = Expression.MakeBinary(ExpressionType.OrElse, leftSide.Predicate.Body, rightInvoke);
            return new Specification<T>(
                                    Expression.Lambda<Func<T, bool>>(newExpression, leftSide.Predicate.Parameters).Expand()
                                  );
        }
    }
}
