using System;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.Messages.Events;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.CommandHandlers
{
    public class PostMessageHandler : WriteSideHandler, IHandleMessages<PostMessageCommand>
    {
        public PostMessageHandler(IBus bus):base(bus)
        {
            NSBHelper.Logger.Info("\r\n~~~~~~~~  PostMessageCommandHandler  ~~~~~~~~~");
        }
        public void Handle(PostMessageCommand message)
        {
            Logger.CommandReceived(message);
            try
            {
                var post = new Post()
                {
                    UserId = message.UserId,
                    Text = message.Text,
                    TimeStamp = message.TimeStamp
                };
                UnitOfWork.PostRepository.Insert(post);
                UnitOfWork.Save();

                var @event = new MessagePostedEvent(post.Text, post.TimeStamp, post.UserId);
                this.Bus.Publish<MessagePostedEvent>(@event);
                Logger.EventPublished(@event);

                //reply success result if sender expects reply
                NSBHelper.Return(this.Bus, CommandResult.Success);
                }
            catch (Exception ex)
            {
                Bus.Return(CommandResult.Error);
                Logger.Error(ex,"Can't insert Post");
            }
           
        }
    }
}