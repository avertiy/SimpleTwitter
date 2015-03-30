using System.Data.Entity;
using System.Linq;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.Data.Repository
{
    public interface IFollowRepository : IRepository<Follow>
    {
        IDbSet<Follow> DbSet { get; set; }
        void Delete(Follow entity);
    }
    public class FollowRepository : GenericRepository<Follow>, IFollowRepository
    {
        public FollowRepository(IDbContext context)
            : base(context)
        {
        }

        public void Delete(Follow entity)
        {
            var entityToDelete =
                Context.Set<Follow>()
                    .FirstOrDefault(f => f.UserId == entity.UserId && f.FollowingUserId == entity.FollowingUserId);
            if (entityToDelete != null)
            {
                DbSet.Remove(entityToDelete);
            }
        }
    }
}