using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleTwitter.WriteSide.Data.Entities
{
    public abstract class Entity : IEntity
    {
        public virtual Guid Id { get; set; }
    }
    public interface IEntity
    {
        Guid Id { get; set; }
    }

    public class User : Entity
    {
        public string UserName { get; set; }
        //public int FollowersCount { get; set; }
        //public int FollowingsCount { get; set; }
        //public int TweetsCount { get; set; }
    }

    /// <summary>
    /// todo rename to tweet
    /// </summary>
    public class Post : Entity
    {
        [MaxLength(140)]
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class Follow : Entity
    {
        public Guid UserId { get; set; }
        public Guid FollowingUserId { get; set; }
    }

}