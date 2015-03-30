using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleTwitter.API.WcfCommandServiceReference;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.ReadSide.Data;
using StackExchange.Redis;

namespace SimpleTwitter.API
{
    internal class SeedHelper
    {
        public static void Seed(bool flushdatabase = false, bool seedanyway = false)
        {
            try
            {
                var connection = ConnectionMultiplexer.Connect(ConfigUtils.GetConfiguration(allowAdmin:flushdatabase));
                var dbnumber = ConfigUtils.GetDatabaseNumber();
                if (flushdatabase)
                {
                    ReadSideService.FlushDatabase(connection, dbnumber);
                }
                var readmodel = new ReadModelFacade(connection.GetDatabase(dbnumber));
                var wcfservice = new WcfCommandServiceClient();

                var seed = flushdatabase || seedanyway;
                if (!seed)
                {
                    Task<bool> dataexists = readmodel.UserExistsAsync("Pete");
                    Task.WaitAny(dataexists);
                    seed = dataexists.Result == false;
                }

                if (seed)
                {
                    var users = new List<CreateUserCommand>(11)
                    {
                        new CreateUserCommand() {UserName = "Pete"},
                        new CreateUserCommand() {UserName = "Mark"},
                        new CreateUserCommand() {UserName = "Andrew"},
                        new CreateUserCommand() {UserName = "Nicolas"},
                        new CreateUserCommand() {UserName = "Anton"},
                        new CreateUserCommand() {UserName = "Ivan"},
                        new CreateUserCommand() {UserName = "Sasha"},
                        new CreateUserCommand() {UserName = "Jens"},
                        new CreateUserCommand() {UserName = "John"},
                        new CreateUserCommand() {UserName = "Sebastian"},
                        new CreateUserCommand() {UserName = "Dolce"}
                    };
                    foreach (var command in users)
                    {
                        wcfservice.CreateUser(command);
                    }

                    var follow = new List<FollowCommand>(10)
                    {
                        new FollowCommand(users[0].UserId, users[1].UserId, true),
                        new FollowCommand(users[0].UserId, users[2].UserId, true),
                        new FollowCommand(users[0].UserId, users[3].UserId, true),
                        new FollowCommand(users[0].UserId, users[4].UserId, true),
                        new FollowCommand(users[0].UserId, users[5].UserId, true),
                        new FollowCommand(users[0].UserId, users[6].UserId, true),
                        new FollowCommand(users[1].UserId, users[7].UserId, true),
                        new FollowCommand(users[2].UserId, users[7].UserId, true),
                        new FollowCommand(users[3].UserId, users[7].UserId, true),
                        new FollowCommand(users[4].UserId, users[7].UserId, true),
                        new FollowCommand(users[2].UserId, users[1].UserId, true),
                        new FollowCommand(users[8].UserId, users[6].UserId, false),
                        new FollowCommand(users[7].UserId, users[6].UserId, false),
                    };
                    foreach (var command in follow)
                    {
                        wcfservice.Follow(command);
                    }
                    
                    var posts = new List<PostMessageCommand>(10)
                    {
                        new PostMessageCommand(users[0].UserId, "my first post"),
                        new PostMessageCommand(users[0].UserId, "my 2nd post"),
                        new PostMessageCommand(users[0].UserId, "my 3rd post"),
                        new PostMessageCommand(users[2].UserId, "1 post"),
                        new PostMessageCommand(users[2].UserId, "2 post"),
                        new PostMessageCommand(users[2].UserId, "3rd post"),
                        new PostMessageCommand(users[3].UserId, "assdasdasd"),
                        new PostMessageCommand(users[4].UserId, "f asf assadsa sdsd "),
                        new PostMessageCommand(users[5].UserId, "sdf sdf wefwe "),
                        new PostMessageCommand(users[6].UserId, "sdfo jsag t983 34erg ger")
                    };
                    foreach (var command in posts)
                    {
                        wcfservice.PostMessage(command);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Seed initial data failed", ex);
            }
        }
    }
}