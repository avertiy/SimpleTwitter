using System;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.Messages.Events;
using SimpleTwitter.ReadSide.Data.DTOs;

namespace SimpleTwitter.ReadSide.EventMessageHandlers
{
  
    public class UserCreatedEventMessageHandler : ReadSideHandler, IHandleMessages<UserCreatedEvent> 
    {
        public UserCreatedEventMessageHandler(IBus bus)
            : base(bus)
        {
            NSBHelper.Logger.Info("\r\n~~~~~~~~  UserCreatedEventMessageHandler  ~~~~~~~~~");
        }
        public void Handle(UserCreatedEvent message)
        {
            Logger.EventReceived(message);
            try
            {
                Service.AddUser(new DTOUser() { Id = message.UserId, UserName = message.UserName });
                NSBHelper.Return(this.Bus, CommandResult.Success);
            }
            catch (Exception ex)
            {
                this.Bus.Return(CommandResult.Error);
                Logger.Error(ex, "Can't insert User");
            }
        }
    }
    public class FollowEventMessageHandler : ReadSideHandler, IHandleMessages<FollowEvent>
    {
        public FollowEventMessageHandler(IBus bus)
            : base(bus)
        {
            NSBHelper.Logger.Info("\r\n~~~~~~~~  FollowEventMessageHandler  ~~~~~~~~~");
        }
        public void Handle(FollowEvent message)
        {
            Logger.EventReceived(message);
            try
            {
                var dto = new DTOFollow() { UserId = message.UserId, FollowingUserId = message.FollowingUserId };
                if (message.Follow)
                {
                    Service.AddFollower(dto);
                    Service.AddFollowing(dto);
                }
                else
                {
                    Service.RemoveFollower(dto);
                    Service.RemoveFollowing(dto);
                }
                NSBHelper.Return(this.Bus, CommandResult.Success);
            }
            catch (Exception ex)
            {
                this.Bus.Return(CommandResult.Error);
                Logger.Error(ex, "Can't process follow event");
            }
        }
    }
    public class MessagePostedEventEventMessageHandler : ReadSideHandler, IHandleMessages<MessagePostedEvent>
    {
        public MessagePostedEventEventMessageHandler(IBus bus)
            : base(bus)
        {
            NSBHelper.Logger.Info("\r\n~~~~~~~~  MessagePostedEventEventMessageHandler  ~~~~~~~~~");
        }
        public void Handle(MessagePostedEvent message)
        {
            Logger.EventReceived(message);
            try
            {
                var dto = new DTOPost() { UserId = message.UserId, Text = message.Text, TimeStamp = message.TimeStamp };
                Service.TweetAsync(dto);
                
                NSBHelper.Return(this.Bus, CommandResult.Success);
            }
            catch (Exception ex)
            {
                this.Bus.Return(CommandResult.Error);
                Logger.Error(ex, "Can't process follow event");
            }
        }
    }
}