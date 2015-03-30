using System.ServiceModel;
using SimpleTwitter.Messages.Commands;

namespace SimpleTwitter.Common
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name 
    //"IService1" in both code and config file together.
    [ServiceContract]
    public interface IWcfCommandService
    {
        [OperationContract]
        void CreateUser(CreateUserCommand command);
        [OperationContract]
        void PostMessage(PostMessageCommand command);
        [OperationContract]
        void Follow(FollowCommand command);
    }
}
