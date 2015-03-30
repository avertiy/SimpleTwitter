using SimpleTwitter.WriteSide.Data.Repository;

namespace SimpleTwitter.WriteSide.Data
{
    public class WriteSideUnitOfWork : UnitOfWork
    {
        public WriteSideUnitOfWork(IWriteSideDbContext dbContext)
            : base(dbContext)
        {
            
        }

        private IUserRepository _userRepository;
        private IPostRepository _postRepository;
        private IFollowRepository _followRepository;

        public IUserRepository UserRepository
        {
            get
            {

                if (this._userRepository == null)
                {
                    this._userRepository = new UserRepository(DbContext);
                }
                return _userRepository;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {

                if (this._postRepository == null)
                {
                    this._postRepository = new PostRepository(DbContext);
                }
                return _postRepository;
            }
        }
        public IFollowRepository FollowRepository
        {
            get
            {

                if (this._followRepository == null)
                {
                    this._followRepository = new FollowRepository(DbContext);
                }
                return _followRepository;
            }
        }
    }
}