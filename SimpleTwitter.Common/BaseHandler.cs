using System;
using NServiceBus;
using SimpleTwitter.Messages;

namespace SimpleTwitter.Common
{
    public abstract class BaseHandler 
    {
        protected readonly IBus Bus;
        
        public ILogger Logger { get; set; }

        protected BaseHandler(IBus bus)
        {
            Bus = bus;
        }

    }
}