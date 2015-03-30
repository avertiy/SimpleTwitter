using System.Data.Entity;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.Data.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IDbSet<User> DbSet { get; set; }
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IDbContext context)
            : base(context)
        {
        }
    }

    
}