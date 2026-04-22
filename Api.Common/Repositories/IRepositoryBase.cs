using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Common.Repositories
{
    public interface IRepositoryBase<T> : IRepository<T> where T : class
    {
        IQueryable<T> Include(string includeProperties = "");
        IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", bool NoTracking = false);

        IList<T> Get(
            Expression<Func<T, bool>> filter = null,
            bool desc = true,
                        string orderBy = null,
            string includeProperties = "");

        IList<T> Get(
                         int take,
            int skip,
           Expression<Func<T, bool>> filter = null,
           bool desc = true,
                       string orderBy = null,
           string includeProperties = "",
           bool NoTracking = false);
    }
}
