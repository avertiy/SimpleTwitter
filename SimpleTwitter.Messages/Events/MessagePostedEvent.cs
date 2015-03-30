using System;

namespace SimpleTwitter.Messages.Events
{
    public class MessagePostedEvent : Event
    {
        public MessagePostedEvent(string text, DateTime timeStamp, Guid userId)
        {
            Text = text;
            TimeStamp = timeStamp;
            UserId = userId;
        }

        public DateTime TimeStamp { get; set; }
        public Guid UserId { get; set; }
        public string Text { get; set; }
        
        public override string ToString()
        {
            return string.Format("Event [MessagePosted: '{0}']", Text);
        }
    }
}