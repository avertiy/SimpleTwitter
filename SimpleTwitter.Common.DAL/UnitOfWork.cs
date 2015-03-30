using System;

namespace SimpleTwitter.WriteSide.Data
{
    public interface IUnitOfWork
    {
        //i don't need Commit and Rollback b/c FakeDbContext is used
        void Save();
    }

    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        protected readonly IDbContext DbContext;
        public UnitOfWork(IDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public void Save()
        {
            DbContext.SaveChanges();
        }

        #region Disposing pattern
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    var disposableResource = DbContext as IDisposable;
                    if (disposableResource != null)
                    {
                        disposableResource.Dispose();
                    }
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
