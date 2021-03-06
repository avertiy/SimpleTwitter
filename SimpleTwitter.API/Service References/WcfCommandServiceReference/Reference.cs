﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleTwitter.API.WcfCommandServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfCommandServiceReference.IWcfCommandService")]
    public interface IWcfCommandService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/CreateUser", ReplyAction="http://tempuri.org/IWcfCommandService/CreateUserResponse")]
        void CreateUser(SimpleTwitter.Messages.Commands.CreateUserCommand command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/CreateUser", ReplyAction="http://tempuri.org/IWcfCommandService/CreateUserResponse")]
        System.Threading.Tasks.Task CreateUserAsync(SimpleTwitter.Messages.Commands.CreateUserCommand command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/PostMessage", ReplyAction="http://tempuri.org/IWcfCommandService/PostMessageResponse")]
        void PostMessage(SimpleTwitter.Messages.Commands.PostMessageCommand command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/PostMessage", ReplyAction="http://tempuri.org/IWcfCommandService/PostMessageResponse")]
        System.Threading.Tasks.Task PostMessageAsync(SimpleTwitter.Messages.Commands.PostMessageCommand command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/Follow", ReplyAction="http://tempuri.org/IWcfCommandService/FollowResponse")]
        void Follow(SimpleTwitter.Messages.Commands.FollowCommand command);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IWcfCommandService/Follow", ReplyAction="http://tempuri.org/IWcfCommandService/FollowResponse")]
        System.Threading.Tasks.Task FollowAsync(SimpleTwitter.Messages.Commands.FollowCommand command);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IWcfCommandServiceChannel : SimpleTwitter.API.WcfCommandServiceReference.IWcfCommandService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class WcfCommandServiceClient : System.ServiceModel.ClientBase<SimpleTwitter.API.WcfCommandServiceReference.IWcfCommandService>, SimpleTwitter.API.WcfCommandServiceReference.IWcfCommandService {
        
        public WcfCommandServiceClient() {
        }
        
        public WcfCommandServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public WcfCommandServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfCommandServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public WcfCommandServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void CreateUser(SimpleTwitter.Messages.Commands.CreateUserCommand command) {
            base.Channel.CreateUser(command);
        }
        
        public System.Threading.Tasks.Task CreateUserAsync(SimpleTwitter.Messages.Commands.CreateUserCommand command) {
            return base.Channel.CreateUserAsync(command);
        }
        
        public void PostMessage(SimpleTwitter.Messages.Commands.PostMessageCommand command) {
            base.Channel.PostMessage(command);
        }
        
        public System.Threading.Tasks.Task PostMessageAsync(SimpleTwitter.Messages.Commands.PostMessageCommand command) {
            return base.Channel.PostMessageAsync(command);
        }
        
        public void Follow(SimpleTwitter.Messages.Commands.FollowCommand command) {
            base.Channel.Follow(command);
        }
        
        public System.Threading.Tasks.Task FollowAsync(SimpleTwitter.Messages.Commands.FollowCommand command) {
            return base.Channel.FollowAsync(command);
        }
    }
}
