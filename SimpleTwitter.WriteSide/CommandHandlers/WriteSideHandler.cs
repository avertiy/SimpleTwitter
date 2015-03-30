using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.WriteSide.Data;

namespace SimpleTwitter.WriteSide.CommandHandlers
{
    public abstract class WriteSideHandler : BaseHandler
    {
        protected WriteSideHandler(IBus bus) : base(bus)
        {
        }
        public WriteSideUnitOfWork UnitOfWork { get; set; }
    }
}