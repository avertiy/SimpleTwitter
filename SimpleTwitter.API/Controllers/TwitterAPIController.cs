using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using SimpleTwitter.API.Models;
using SimpleTwitter.API.WcfCommandServiceReference;
using SimpleTwitter.Messages.Commands;
using SimpleTwitter.ReadSide.Data;
using SimpleTwitter.ReadSide.Data.DTOs;
using StackExchange.Redis;

namespace SimpleTwitter.API.Controllers
{
     //[EnableQuery]
     //[ODataRoutePrefix("twitterapi")]
    [RoutePrefix("api/twitter")]
    public class TwitterAPIController : ApiController//ODataController
     {
         private readonly IWcfCommandService _wcfservice = new WcfCommandServiceClient();
         private readonly IReadModelFacade _readmodel;
         private static ConnectionMultiplexer _connection;

         public TwitterAPIController()
         {
             if (_connection == null)
             {
                 _connection = ConnectionMultiplexer.Connect(ConfigUtils.GetConfiguration());
             }
             _readmodel = new ReadModelFacade(_connection.GetDatabase(ConfigUtils.GetDatabaseNumber()));
         }
        
         [HttpGet, Route("users")]
         public async Task<IHttpActionResult> GetUsers()
         {
             try
             {
                 var dtousers = await _readmodel.GetUsersAsync(_connection);
                 //map dto objects to models
                 var users = dtousers.Select(dto => new UserModel(dto)).OrderBy(u=>u.UserName);
                 return Ok(users);
             }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
         }
         [HttpGet, Route("users/{userId}")]
         public async Task<IHttpActionResult> GetUser([FromUri] Guid userId)
         {
             try
             {
                 var task1 = _readmodel.FindUserAsync(userId);
                 var task2 = _readmodel.GetFollowersAsync(userId);
                 var task3 = _readmodel.GetFollowingsAsync(userId);

                 var dto = await task1;
                 if (dto == null)
                 {
                     return BadRequest(string.Format("user [id:{0}] doesn't exist", userId));
                 }
                 var model = new UserModel(dto) { Followers = (await task2).Users, Followings = (await task3).Users };
                 return Ok(model);
             }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
         }

         [HttpPut, Route("follow")]
         public async Task<IHttpActionResult> Follow([FromBody] FollowRequestModel request)
         {
             return await Follow(request, true);
         }
         [HttpPut, Route("unfollow")]
         public async Task<IHttpActionResult> UnFollow([FromBody] FollowRequestModel request)
         {
             return await Follow(request, false);
         }
         private async Task<IHttpActionResult> Follow(FollowRequestModel request, bool isfollow)
        {
            try
            {
                if (request.UserId == request.FollowingId)
                {
                    return BadRequest("User can't follow yourself");
                }
                var task1 = _readmodel.FindUserAsync(request.UserId);
                var task2 = _readmodel.FindUserAsync(request.FollowingId);
                var user = await task1;
                if (user == null)
                {
                    return BadRequest(string.Format("User [id:{0}] doesn't exist", request.UserId));
                }
                var followingUser = await task2;
                if (followingUser == null)
                {
                    return BadRequest(string.Format("User [id:{0}] doesn't exist", request.FollowingId));
                }
                var cmd = new FollowCommand(request.FollowingId, request.UserId, isfollow);
                if (!cmd.IsValid())
                {
                    return BadRequest("Command validation is not passed");
                }
                await _wcfservice.FollowAsync(cmd);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
         
         [HttpPost, Route("users")]
         public async Task<IHttpActionResult> Post([FromBody]UserModel user)
         {
             try
             {
                 if (!ModelState.IsValid)
                 {
                     return BadRequest(ModelState);
                 }
                 var exists = await _readmodel.UserExistsAsync(user.UserName);
                 if (exists)
                 {
                     return Conflict();
                 }
                 var cmd = new CreateUserCommand() {UserName = user.UserName};
                 if (!cmd.IsValid())
                 {
                     throw new ValidationException("Command is not valid");
                 }
                 user.Id = cmd.UserId;
                 await _wcfservice.CreateUserAsync(cmd);
                 return Created("api/twitter/users/" + user.Id, user);
             }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
         }
        
         [HttpPost, Route("tweet")]
         public async Task<IHttpActionResult> PostMessage([FromBody]PostModel model)
         {
             try
             {
                 if (!ModelState.IsValid)
                 {
                     return BadRequest(ModelState);
                 }
                 var user = await _readmodel.FindUserAsync(model.UserId);
                 if (user == null)
                 {
                     return BadRequest(string.Format("User [id:'{0}'] doesn't exist", model.UserId));
                 }
                 var cmd = new PostMessageCommand(model.UserId, model.MessageText);
                 if (!cmd.IsValid())
                 {
                     return BadRequest("Command validation is not passed");
                 }
                 await _wcfservice.PostMessageAsync(cmd);
                 model.TimeStamp = cmd.TimeStamp;
                 model.Id = cmd.Id;
                 return Created<PostModel>("twitterapi", model);
             }
             catch (Exception ex)
             {
                 return InternalServerError(ex);
             }
         }

        [HttpGet, Route("feeds/{UserId}")]
         public async Task<IHttpActionResult> GetFeeds([FromUri]GetFeedRequestModel model)
        {
            try
            {
                if (!model.UserId.HasValue)
                {
                    return BadRequest(ModelState);
                }
                var response = await
                    _readmodel.GetFeedAsync(new DTOGetFeedRequest()
                    {
                        UserId = model.UserId.Value,
                        Start = model.Start,
                        Stop = model.Stop,
                        Order = Order.Ascending
                    });
                var feed = response.Feed.OrderByDescending(dto=>dto.TimeStamp).Select(dto => new PostModel(dto));
                return Ok(feed);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("globalfeed/{Start}")]
        public async Task<IHttpActionResult> GetGlobalFeed([FromUri]GetFeedRequestModel model)
        {
            try
            {
                var response = await
                    _readmodel.GetGlobalFeedAsync(new DTOGetFeedRequest()
                    {
                        Start = model.Start,
                        Stop = model.Stop,
                        Order = Order.Ascending
                    });
                var feed = response.Feed.Select(dto => new PostModel(dto));
                return Ok(feed);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


    }

   
}

