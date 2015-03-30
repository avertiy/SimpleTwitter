using System;
using System.Web.Security;
using NServiceBus;
using SimpleTwitter.Common;
using SimpleTwitter.Messages;
using SimpleTwitter.Messages.Commands;

namespace SimpleTwitter.CommandServiceBus
{
    public class WcfCommandService : IWcfCommandService
    {
        private readonly IBus _bus;
        private static int _counter = 1;

        public WcfCommandService()
        {
#if DEBUG
            Console.WriteLine(" ~~~~~~~~~~~  WcfCommandService instantiated [counter #{0}]  ~~~~~~~~~~~",_counter++);
#endif
            _bus = NSBHelper.BusInstance;
        }
        public void CreateUser(CreateUserCommand command)
        {
            NSBHelper.Send(_bus, NSBHelper.ENDPOINT_WRITESIDE, command);
        }

        public void PostMessage(PostMessageCommand command)
        {
            NSBHelper.Send(_bus, NSBHelper.ENDPOINT_WRITESIDE, command);
        }

        public void Follow(FollowCommand command)
        {
            NSBHelper.Send(_bus, NSBHelper.ENDPOINT_WRITESIDE, command);
        }
    }
}
