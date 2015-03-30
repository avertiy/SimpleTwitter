using System;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.Messages.Events;
using SimpleTwitter.WriteSide.Data.Entities;

namespace SimpleTwitter.WriteSide.CommandHandlers
{

    public class CreateUserCommandHandler : WriteSideHandler, IHandleMessages<CreateUserCommand>
    {
        public CreateUserCommandHandler(IBus bus)
            : base(bus)
        {
            NSBHelper.Logger.Info("\r\n~~~~~~~~  RequestDataMessageHandler  ~~~~~~~~~");
        }
        public void Handle(CreateUserCommand message)
        {
            Logger.CommandReceived(message);
            try
            {
                var user = new User() { UserName = message.UserName, Id = message.UserId };
                UnitOfWork.UserRepository.Insert(user);
                UnitOfWork.Save();

                //I decided no to use CQRS +Event sourcing with Event Store just CQRS pattern
                //as there is no event store that will create and publish event
                //create the event here and publish it
                var @event = new UserCreatedEvent(user.Id, user.UserName);
                try
                {
                    this.Bus.Publish<UserCreatedEvent>(@event);
                    Logger.EventPublished(@event);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Failed to publish event " + @event);
                }

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