using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using SimpleTwitter.ReadSide.Data.DTOs;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data
{
    public interface IReadModelFacade
    {
        Task<bool> UserExistsAsync(string username);
        Task<DTOUser> FindUserAsync(Guid userid);
        Task<IEnumerable<DTOUser>> GetUsersAsync(ConnectionMultiplexer connection);
        Task<DTOFeedResponse> GetFeedAsync(DTOGetFeedRequest request);
        Task<DTOFeedResponse> GetGlobalFeedAsync(DTOGetFeedRequest request);
        Task<DTOGetFollowersResponse> GetFollowingsAsync(Guid userid);
        Task<DTOGetFollowersResponse> GetFollowersAsync(Guid userid);
    }
    public class ReadModelFacade : ReadSideService, IReadModelFacade
    {
        public ReadModelFacade(IDatabase db)
            : base(db)
        {
        }
        public Task<bool> UserExistsAsync(string username)
        {
            return UserIdsByName.KeyExistsAsync(username);
        }

        public async Task<DTOUser> FindUserAsync(Guid userid)
        {
            var username = await UserNamesById.GetAsync(userid.ToString());
            if (string.IsNullOrEmpty(username)) return null;
            return new DTOUser() { Id = userid, UserName = username};
        }
        public async Task<IEnumerable<DTOUser>> GetUsersAsync(ConnectionMultiplexer connection)
        {
            //it is impossible to use Database as the Database is logical but the KEYS operation is pefromed on a certain server 
            //https://github.com/StackExchange/StackExchange.Redis/blob/master/Docs/KeysScan.md
            //so it is needed to scan each server
            var endpoints = connection.GetEndPoints();
            var tasks = new List<Task<DTOUser>>();
            foreach (EndPoint endpoint in endpoints)
            {
                var srv = connection.GetServer(endpoint);
                var keys = srv.Keys(pattern: UserNamesById.UrnPrefix + "*");
                tasks.AddRange(keys.Select(GetDTOUserAsync));
            }
            return await Task.WhenAll(tasks);
        }
        private async Task<DTOUser> GetDTOUserAsync(RedisKey key)
        {
            try
            {
                var username = (string)await Db.StringGetAsync(key);
                var id = (string) await UserIdsByName.GetAsync(username);

                return new DTOUser() {Id = Guid.Parse(id), UserName = username};
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DTOFeedResponse> GetFeedAsync(DTOGetFeedRequest request)
        {
            //Guid userid, long start = 0, long stop = -1, Order order = Order.Ascending
            var res = await RecentFeeds.RangeByRankAsync(request.UserId.ToString(), request.Start, request.Stop, request.Order);
            var response = new DTOFeedResponse()
            {
                UserId = request.UserId,
                Feed = res.Select(ParseFeedValue)
            };
            return response;
        }

        public async Task<DTOFeedResponse> GetGlobalFeedAsync(DTOGetFeedRequest request)
        {
            var res = await GlobalFeed.RangeByRankAsync(CURRENT_GLOBAL_FEED_KEY, request.Start, request.Stop, request.Order);
            var response = new DTOFeedResponse()
            {
                Feed = res.Select(ParseFeedValue)
            };
            return response;
        }

        public async Task<DTOGetFollowersResponse> GetFollowersAsync(Guid userid)
        {
            var ids =  await Followers.RangeByRankAsync(userid.ToString());
            var list = new List<KeyValuePair<string, string>>();
            foreach (var key in ids)
            {
                string username = await UserNamesById.GetAsync(key);
                list.Add(new KeyValuePair<string, string>(key,username));
            }
            DTOGetFollowersResponse response = new DTOGetFollowersResponse{Users = list};
            return response;
        }
        public async Task<DTOGetFollowersResponse> GetFollowingsAsync(Guid userid)
        {
            var ids = await Followings.RangeByRankAsync(userid.ToString());
            var list = new List<KeyValuePair<string, string>>();
            foreach (var key in ids)
            {
                string username = await UserNamesById.GetAsync(key);
                list.Add(new KeyValuePair<string, string>(key, username));
            }
            DTOGetFollowersResponse response = new DTOGetFollowersResponse { Users = list };
            return response;
        }
    }
}