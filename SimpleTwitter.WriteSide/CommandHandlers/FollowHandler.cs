using System;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.Messages.Events;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.CommandHandlers
{
    public class FollowHandler : WriteSideHandler, IHandleMessages<FollowCommand>
    {
        public FollowHandler(IBus bus)
            : base(bus)
        {
            NSBHelper.Logger.Info("~~~~~~~~  FollowCommandHandler  ~~~~~~~~~");
        }
        public void Handle(FollowCommand command)
        {
            Logger.CommandReceived(command);
            try
            {
                var entity = new Follow() { UserId = command.UserId, FollowingUserId = command.FollowingUserId };
                FollowEvent @event = new FollowEvent(command.UserId, command.FollowingUserId, command.Follow);
                if (command.Follow)
                {
                    UnitOfWork.FollowRepository.Insert(entity);
                    UnitOfWork.Save();
                    
                }
                else
                {
                    UnitOfWork.FollowRepository.Delete(entity);
                    UnitOfWork.Save();
                }
                this.Bus.Publish(@event);
                this.Logger.EventPublished(@event);
                
                //reply success result if sender expects reply
                NSBHelper.Return(this.Bus, CommandResult.Success);
            }
            catch (Exception ex)
            {
                //reply error result if sender expects reply
                this.Bus.Return(CommandResult.Error);
                Logger.Error(ex, "Can't insert User");
            }
        }
    }
}