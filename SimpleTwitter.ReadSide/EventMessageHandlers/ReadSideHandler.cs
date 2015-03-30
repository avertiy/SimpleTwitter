using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.ReadSide.Data;

namespace SimpleTwitter.ReadSide.EventMessageHandlers
{
    public abstract class ReadSideHandler : BaseHandler
    {
        protected ReadSideHandler(IBus bus)
            : base(bus)
        {
        }
        public ReadSideService Service { get; set; }
    }
}