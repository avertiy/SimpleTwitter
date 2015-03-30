using System.Data.Entity;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.Data.Repository
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public IDbContext Context { get; set; }
        public IDbSet<TEntity> DbSet { get; set; }

        public GenericRepository(IDbContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }
        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void Update(TEntity entity)
        {
            var existingObject = DbSet.Find(entity.Id);
            //var existingObject = DbSet.Find(entity.GetPrimaryKey());
            if (existingObject != null)
            {
                DbSet.Remove(existingObject);
            }
            DbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = DbSet.Find(id);
            DbSet.Remove(entityToDelete);
        }
    }

}