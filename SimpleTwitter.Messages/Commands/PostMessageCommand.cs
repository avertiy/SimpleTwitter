using System;

namespace SimpleTwitter.Messages.Commands
{
    public class PostMessageCommand : Command
    {
        private string _text;

        public PostMessageCommand()
        {
        }

        public PostMessageCommand(Guid userId, string text)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Text = text;
            TimeStamp = DateTime.Now;
        }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public string Text
        {
            get { return _text; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("value");
                }
                if (value.Length > 140)
                {
                    throw new ArgumentOutOfRangeException("value");
                }   
                _text = value;
            }
        }

        public DateTime TimeStamp { get; set; }
        
        public override bool IsValid()
        {
            return !(string.IsNullOrEmpty(Text) || Text.Length > Constansts.TWEET_LENGTH);
        }

        public override string ToString()
        {
            return string.Format("Command [PostMessage  Text '{0}' Timestamp {1:MMM dd HH:mm:ss}", Text,TimeStamp);
        }
    }
}