using System;
using NServiceBus;

namespace SimpleTwitter.Common
{
    public interface ILogger
    {
        void CommandReceived(ICommand command);
        void EventReceived(IEvent command);
        void CommandSent(ICommand command, string endpoint);
        void EventPublished(IEvent command);
        void Return(object result);
        void CommandExecutionResult(object result);
        void Error(Exception exception, string message);
        void Info(string format, params object[] args);
    }
    public class SimpleConsoleLogger : ILogger
    {
        //todo refactoring: move all methods to NSBHelper, leave in logger only Info,Log, and Error methods
        public void CommandReceived(ICommand command)
        {
            Console.WriteLine("RECEIVED {0}", command);
        }
        public void EventReceived(IEvent @event)
        {
            Console.WriteLine("RECEIVED {0}", @event);
        }

        public void CommandSent(ICommand command, string endpoint)
        {
            Console.WriteLine("SENT {0} TO [{1}]", command, endpoint);
        }
        public void EventPublished(IEvent @event)
        {
            Console.WriteLine("PUBLISHED {0}", @event);
        }
        public void CommandExecutionResult(object result)
        {
            Console.WriteLine("RESULT: {0}", result);
        }

        public void Error(Exception error, string message)
        {
            Console.WriteLine("ERROR: {0}", error.Message);
        }

        public void Info(string format, params object[] args)
        {
            Console.WriteLine(format, args);
        }

        public void Return(object result)
        {
            Console.WriteLine("REPLIED {0}", result);
        }
    }
}