using System;

namespace SimpleTwitter.Messages.Commands
{
    public class CreateUserCommand : Command
    {
        public CreateUserCommand()
        {
            UserId = Guid.NewGuid();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        
        //public string Email { get; set; }
        public override bool IsValid()
        {
            return !(string.IsNullOrEmpty(UserName));
        }

        public override string ToString()
        {
            return string.Format("Command [CreateUser   userid [..{0}]  username {1}]", UserId.ToString().Remove(0, 30), UserName);
        }
    }
}