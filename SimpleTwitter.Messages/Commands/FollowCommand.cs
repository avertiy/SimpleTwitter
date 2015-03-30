using System;

namespace SimpleTwitter.Messages.Commands
{
    public class FollowCommand : Command
    {
        public FollowCommand()
        {
        }

        public FollowCommand(Guid followingUserId, Guid userId, bool follow)
        {
            FollowingUserId = followingUserId;
            UserId = userId;
            Follow = follow;
        }

        public Guid FollowingUserId { get; set; }
        public Guid UserId { get; set; }
        /// <summary>
        /// True means follow command, False means unfollow command
        /// </summary>
        public bool Follow { get; set; }
        public override bool IsValid()
        {
            return UserId != FollowingUserId;
        }
        public override string ToString()
        {
            return string.Format(Follow ? 
                "Command [Follow  UserId [..{0}]  FollowingUserId [..{1}]]" : 
                "Command [UnFollow  UserId [..{0}]  FollowingUserId [..{1}]]", 
                UserId.ToString().Remove(0, 30), FollowingUserId.ToString().Remove(0, 30));
        }
    }
}