using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

namespace SimpleTwitter.Messages
{
    public static class Constansts
    {
        public const int TWEET_LENGTH = 140;
    }
    class ConfigErrorQueue : IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
    {
        public MessageForwardingInCaseOfFaultConfig GetConfiguration()
        {
            return new MessageForwardingInCaseOfFaultConfig
            {
                ErrorQueue = "error"
            };
        }
    }
}