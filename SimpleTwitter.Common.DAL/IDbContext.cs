using System;
using System.Data.Entity;
using SimpleTwitter.WriteSide.Data.Entities;
using SimpleTwitter.WriteSide.Data.Repository;

namespace SimpleTwitter.WriteSide.Data
{
    public interface IDbContext
    {
        int SaveChanges();
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
    public interface IWriteSideDbContext : IDbContext
    {
        IDbSet<User> Users { get; }
        IDbSet<Post> Posts { get; }
    }
    public class WriteSideDbFakeDbContext : IWriteSideDbContext
    {
        public WriteSideDbFakeDbContext()
        {
            Users = new FakeDbSet<User>();
            Posts = new FakeDbSet<Post>();
            Follows = new FakeDbSet<Follow>();
        }

        public IDbSet<User> Users { get; private set; }
        
        /// <summary>
        /// Tweets
        /// </summary>
        public IDbSet<Post> Posts { get; private set; }

        public IDbSet<Follow> Follows { get; private set; }

        public int SaveChanges()
        {
            return 0;
        }

        public IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            if (typeof(TEntity) == typeof(User)) return (IDbSet<TEntity>)Users;
            if (typeof(TEntity) == typeof(Post)) return (IDbSet<TEntity>)Posts;
            if (typeof(TEntity) == typeof(Follow)) return (IDbSet<TEntity>)Follows;
            throw new NotImplementedException();
        }
    }
}