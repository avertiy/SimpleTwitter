using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.Data.Repository
{
    public interface IPostRepository : IRepository<Post>
    {

    }
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        public PostRepository(IDbContext context)
            : base(context)
        {
        }
    }
}