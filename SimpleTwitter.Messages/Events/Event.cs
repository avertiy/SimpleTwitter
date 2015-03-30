using System;
using NServiceBus;

namespace SimpleTwitter.Messages.Events
{
    public abstract class Event : IEvent
    {
        //public int Version;
    }
    public class UserCreatedEvent : Event
    {
        public UserCreatedEvent(Guid userId, string userName)
        {
            UserId = userId;
            UserName = userName;
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        
        public override string ToString()
        {
            return string.Format("Event [UserCreated: userid [..{0}]  username {1}]", UserId.ToString().Remove(0,30), UserName);
        }
    }
    public class FollowEvent : Event
    {
        public FollowEvent(Guid userId, Guid followingUserId, bool follow)
        {
            UserId = userId;
            FollowingUserId = followingUserId;
            Follow = follow;
        }

        public Guid UserId { get; set; }
        public Guid FollowingUserId { get; set; }
        public bool Follow { get; set; }

        public override string ToString()
        {
            return string.Format(Follow ? 
                "Event [Follow user [..{0}]  following user [..{1}]]" : 
                "Event [UnFollow  user [..{0}]  following user [..{1}]]",
                UserId.ToString().Remove(0, 30), FollowingUserId.ToString().Remove(0, 30));
        }
    }
}