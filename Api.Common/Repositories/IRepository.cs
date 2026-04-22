using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Common.Repositories
{
    public interface IRepository<T> : IQueryable<T> where T : class
    {


        DbSet<T> GetDbSet();

        DbContext GetDbContext();

        IList<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");

        IList<T> Get(
             Expression<Func<T, bool>> filter = null,
             string includeProperties = "");

        IList<T> GetList(
             int take,
            int skip,
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "", bool NoTracking = false);
        IList<T> GetList(IQueryable<T> query,
           int take,
           int skip,
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
        IList<T> GetList(
           int take,
           int skip,
          Expression<Func<T, bool>> filter = null,
          string orderBy = null,
          bool desc = true,
          string includeProperties = "");
        IList<T> GetList<TKey>(
            int take,
            int skip,
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "", bool NoTracking = false, Func<T, TKey> distinctBy = null);
        void Add(T entity);
        void AddRange(List<T> entity);
        void UpdateRange(List<T> entityList);

        void Delete(T entity);

        void DeleteRange(List<T> entityList);

        void Delete(Expression<Func<T, bool>> where);

        void Save();

        //T GetById(int id);
        //T GetById(long id);

        //T GetById(decimal id);

        //T GetById(string id);

        //T Find(params object[] keys);

        T Find(object key);

        void Reload(T entity);

        IEnumerable<T> GetAll();

        IEnumerable<T> Get(Expression<Func<T, bool>> where);
        //T Get(Expression<Func<T, bool>> where);

        void AddOrUpdate(T entity, object id);

        //List<T> Query(ISpecification<T> specification);

    }
}
