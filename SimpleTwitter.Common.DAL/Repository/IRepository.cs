namespace SimpleTwitter.WriteSide.Data.Repository
{
    public interface IRepository<TEntity> where TEntity:class
    {
        IDbContext Context { get; set; }
        void Insert(TEntity entity);

        void Update(TEntity entity);
        void Delete(object id);
        TEntity GetById(object id);
    }
}