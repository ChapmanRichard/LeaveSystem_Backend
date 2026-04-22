using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Api.Common.Repositories
{
    public abstract class UnitOfWorkBase : IDisposable
    {

        public DbContext _dbContext;


        public UnitOfWorkBase()
        {
            this._dbContext = null;

        }
        public void Save()
        {
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save 
                    // from the store
                    ex.Entries.Single().Reload();
                }
                catch (DbUpdateException)
                {

                    throw;
                }
                catch (Exception)
                {

                    throw;
                }
            } while (saveFailed);
        }


        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}
