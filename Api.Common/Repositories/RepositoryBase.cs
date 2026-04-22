using Api.Common.Linq;
using LinqKit.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Api.Common.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //internal ProgrammingEFDB1Entities _dataContext;
        protected DbContext _dataContext;
        protected DbSet<T> _dbset;

        protected IQueryable<T> RepositoryQuery { get; set; }

        //public CoreCashRepositoryBase()
        //{
        //    CoreCashContext context = new CoreCashContext();
        //    this._dataContext = context;
        //    this._dbset = context.Set<T>();
        //}

        public RepositoryBase(DbContext context)
        {
            this._dataContext = context;
            this._dataContext = ((DbContext)context);
            this._dbset = this._dataContext.Set<T>();
            this.RepositoryQuery = this._dbset.AsQueryable();
        }



        public void SetDb(DbContext context)
        {
            this._dataContext = context;
            this._dbset = context.Set<T>();
        }

        public DbContext GetDbContext()
        {
            return this._dataContext;
        }

        public DbSet<T> GetDbSet()
        {

            return this._dbset;
        }


        //public T Find(params object[] keys)
        //{
        //    return this._dbset.Find(keys);
        //}

        public T Find(object key)
        {
            return this._dbset.Find(key);
        }

        public void Reload(T entity)
        {
            this._dataContext.Entry(entity).Reload();
        }

        //public T Find(string key)
        //{
        //    return this._dbset.Find(key);
        //}

        public virtual IQueryable<T> Include(string includeProperties = "")
        {
            IQueryable<T> query = _dbset;
            //return query.Include(includeProperties);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public virtual IQueryable<T> GetQueryable(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", bool NoTracking = false)
        {
            IQueryable<T> query = _dbset;
            if (NoTracking)
            {
                query = query.AsNoTracking();
            }
            //_dbset.Include
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        public virtual IList<T> Get(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbset;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public virtual IList<T> Get(
            Expression<Func<T, bool>> filter = null,
             bool desc = true,
                        string orderBy = null,

            string includeProperties = "")
        {
            IQueryable<T> query = _dbset;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (desc)
                {
                    query = query.OrderByDescending(p => EF.Property<T>(p, orderBy));
                }
                else
                {
                    query = query.OrderBy(p => EF.Property<T>(p, orderBy));
                }
            }

            return query.ToList();
        }
        public virtual IList<T> Get(
                                     int take,
            int skip,
            Expression<Func<T, bool>> filter = null,
             bool desc = true,
                        string orderBy = null,

            string includeProperties = "",
            bool NoTracking = false)
        {
            IQueryable<T> query = _dbset;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                if (desc)
                {
                    query = query.OrderByDescending(p => EF.Property<T>(p, orderBy));
                }
                else
                {
                    query = query.OrderBy(p => EF.Property<T>(p, orderBy));
                }
            }
            if (NoTracking)
            {
                return query.Skip(skip).Take(take).AsNoTracking().ToList();
            }
            else
            {
                return query.Skip(skip).Take(take).ToList();
            }

            //return query.ToList();
        }

        public virtual IList<T> Get(
          Expression<Func<T, bool>> filter = null,
          string includeProperties = "")
        {
            IQueryable<T> query = _dbset;



            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            return query.ToList();

        }

        public virtual IList<T> GetNoTracking(
         Expression<Func<T, bool>> filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
         string includeProperties = "")
        {
            IQueryable<T> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty).AsNoTracking();
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IList<T> GetList(
           int take,
           int skip,
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
          string includeProperties = "", bool NoTracking = false)
        {
            IQueryable<T> query = _dbset;



            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (NoTracking)
            {
                query = query.AsNoTracking();
            }

            if (take > 0)
                return query.Skip(skip).Take(take).ToList();
            else
            {
                return query.Skip(skip).ToList();
            }

        }
        public IList<T> GetList<TKey>(int take, int skip, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "", bool NoTracking = false, Func<T, TKey> distinctBy = null)
        {
            IQueryable<T> query = _dbset;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (NoTracking)
            {
                query = query.AsNoTracking();
            }
            if (distinctBy != null)
            {
                query = query.Distinct(distinctBy).AsQueryable();
            }
            if (take > 0)
                return query.Skip(skip).Take(take).ToList();
            else
            {
                return query.Skip(skip).ToList();
            }
        }
        public virtual IList<T> GetList(IQueryable<T> query,
           int take,
           int skip,
          Expression<Func<T, bool>> filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
        {
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Skip(skip).Take(take).ToList();

        }

        public IList<T> GetList(
            int take,
            int skip,
            Expression<Func<T, bool>> filter = null,
            string orderBy = null,
            bool desc = true,
            string includeProperties = "")
        {
            IQueryable<T> query = _dbset;
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                if (desc)
                {
                    query = query.OrderByDescending(p => EF.Property<T>(p, orderBy));
                }
                else
                {
                    query = query.OrderBy(p => EF.Property<T>(p, orderBy));
                }
            }
            return query.Skip(skip).Take(take).ToList();

        }



        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Attach(T entity)
        {
            _dbset.Attach(entity);

        }
        public virtual void AddRange(List<T> entity)
        {
            _dbset.AddRange(entity);
        }
        public virtual void UpdateRange(List<T> entityList)
        {
            _dbset.UpdateRange(entityList);
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public virtual void DeleteRange(List<T> entityList)
        {
            _dbset.RemoveRange(entityList);
        }

        public virtual void Delete(int id)
        {
            var data = _dbset.Find(id);

            if (data != null)
            {
                _dbset.Remove(data);
            }
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }


        //public virtual T GetById(int id)
        //{
        //    return _dbset.Find(id);
        //}

        //public virtual T GetById(long id)
        //{
        //    return _dbset.Find(id);
        //}

        //public virtual T GetById(decimal id)
        //{
        //    return _dbset.Find(id);
        //}
        //public virtual T GetById(string id)
        //{
        //    return _dbset.Find(id);
        //}
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }

        public virtual IList<T> GetList()
        {
            return _dbset.ToList();
        }

        public virtual IQueryable<T> GetQueryable()
        {
            return _dbset.AsQueryable();
        }

        //public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        //{
        //    return _dbset.Where(where).ToList();
        //}
        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }

        public virtual IEnumerable<T> Where(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }

        public void Save()
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    _dataContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save 
                    // from the store
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }

        public virtual void AddOrUpdate(T entity, object id)
        {
            var data = _dbset.Find(id);

            if (data == null)
            {
                _dbset.Add(entity);
            }
            else
            {

                _dataContext.Entry(entity).State = EntityState.Modified;

            }
        }

        public virtual void Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void AddIfNotExist(T entity, int id)
        {
            var data = _dbset.Find(id);

            if (data == null)
            {
                _dbset.Add(entity);
            }
            else
            {

                //_dataContext.Entry(entity).State = EntityState.Modified;

            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return RepositoryQuery.GetEnumerator();
        }














        //#endregion

        #region Implementation of IQueryable
        /// <summary>
        /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.
        /// </returns>
        public Expression Expression
        {
            get { return RepositoryQuery.Expression; }
        }
        /// <summary>
        /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.
        /// </returns>
        public Type ElementType
        {
            get { return RepositoryQuery.ElementType; }
        }
        /// <summary>
        /// Gets the query provider that is associated with this data source.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.
        /// </returns>
        public IQueryProvider Provider
        {
            get { return RepositoryQuery.Provider; }
        }
        #endregion


    }
}
