using NServiceBus;

namespace SimpleTwitter.Messages.Commands
{
    public abstract class Command : ICommand
    {
        public abstract bool IsValid();
    }
}
