using System;
using System.Globalization;
using SimpleTwitter.ReadSide.Data.DTOs;
using SimpleTwitter.ReadSide.Data.Redis;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data
{
    
    public class ReadSideService 
    {
        public const string CURRENT_GLOBAL_FEED_KEY = "current";
        public const string DATETIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        
        #region fields
        protected readonly IDatabase Db;
        private KVStringSet _userIdsByName;
        private KVStringSet _userNamesById;
        //as users guids are stored the simple redis set can be used instead of a sorted set
        private KVSortedSet _followers;
        //as users guids are stored the simple redis set can be used instead of a sorted set
        private KVSortedSet _followings;
        private KVSortedSet _recentFeeds;
        private KVSortedSet _globalFeed; 
        #endregion
        
        #region Prop-s
        protected KVStringSet UserIdsByName
        {
            get
            {

                if (this._userIdsByName == null)
                {
                    this._userIdsByName = new KVStringSet(Db, "urn:UserIdsByName");
                }
                return _userIdsByName;
            }
        }
        protected KVStringSet UserNamesById
        {
            get
            {
                //Followers.RangeByRank()
                if (this._userNamesById == null)
                {
                    this._userNamesById = new KVStringSet(Db, "urn:UserNamesById");
                }
                return _userNamesById;
            }
        }

        /// <summary>
        /// user with id is followed by users
        /// </summary>
        protected KVSortedSet Followers
        {
            get
            {

                if (this._followers == null)
                {
                    this._followers = new KVSortedSet(Db, "urn:sset:Followers");
                }
                return _followers;
            }
        }
        protected KVSortedSet Followings
        {
            get
            {

                if (this._followings == null)
                {
                    this._followings = new KVSortedSet(Db, "urn:sset:Followings");
                }
                return _followings;
            }
        }

        protected KVSortedSet RecentFeeds
        {
            get
            {

                if (this._recentFeeds == null)
                {
                    this._recentFeeds = new KVSortedSet(Db, "urn:sset:RecentFeeds");
                }
                return _recentFeeds;
            }
        }

        protected KVSortedSet GlobalFeed
        {
            get
            {

                if (this._globalFeed == null)
                {
                    this._globalFeed = new KVSortedSet(Db, "urn:sset:GlobalFeed");
                }
                return _globalFeed;
            }
        } 
        #endregion
        public ReadSideService(IDatabase db)
        {
            Db = db;
        }

        public void AddUser(DTOUser user)
        {
            UserIdsByName.Set(user.UserName, user.Id.ToString());
            UserNamesById.Set(user.Id.ToString(), user.UserName);
        }
        public async void TweetAsync(DTOPost post)
        {
            var username = await UserNamesById.GetAsync(post.UserId.ToString());
            var utc = post.TimeStamp.ToUniversalTime();
            var score = GetScore(utc);
            string body = string.Format("$userid${0}$username${1}$timestamp${2}$text${3}", post.UserId, username, utc.ToString(DATETIME_FORMAT), post.Text);
            RecentFeeds.Add(post.UserId.ToString(), score, body);
            var followers = await Followers.RangeByRankAsync(post.UserId.ToString());
            foreach (var follower in followers)
            {
                RecentFeeds.Add(follower, score, body);
            }
            AddPostToGlobalFeedAsync(post);
        }
        protected DTOPost ParseFeedValue(string value)
        {
            var entries = value.Split(new[] { "$userid$", "$username$", "$timestamp$", "$text$" }, StringSplitOptions.RemoveEmptyEntries);
            if (entries.Length != 4)
            {
                throw new ArgumentException(value);
            }
            var userid = Guid.Parse(entries[0]);
            var username = entries[1];
            var timestamp = DateTime.ParseExact(entries[2], DATETIME_FORMAT, CultureInfo.InvariantCulture);
            string text = entries[3];
            return new DTOPost() {UserId = userid,UserName = username, Text = text, TimeStamp = timestamp};
        }

        protected async void AddPostToGlobalFeedAsync(DTOPost post)
        {
            var utc = post.TimeStamp.ToUniversalTime();
            var username = await UserNamesById.GetAsync(post.UserId.ToString());
            string body = string.Format("$userid${0}$username${1}$timestamp${2}$text${3}", post.UserId,username, utc.ToString(DATETIME_FORMAT), post.Text);
            //key current may contain upto 9999 latest posts and when it reaches the bound 
            //we can use redis trim feature for sets or just change key name from 'current' to 
            //'archived#YYYYMMDD' and start new set with a key current
            GlobalFeed.Add(CURRENT_GLOBAL_FEED_KEY, GetScore(utc), body);
        }

        /// <returns>True if success, False if Following user is already followed by the user</returns>
        public bool AddFollower(DTOFollow dto)
        {
            return Followers.Add(dto.FollowingUserId.ToString(), dto.UserId.ToString()) == 1;
        }
        /// <returns>True if success, False if user already follows the following user</returns>
        public bool AddFollowing(DTOFollow dto)
        {
            return Followings.Add(dto.UserId.ToString(), dto.FollowingUserId.ToString()) == 1;
        }

        public bool RemoveFollower(DTOFollow dto)
        {
            return Followers.Remove(dto.FollowingUserId.ToString(), dto.UserId.ToString());
        }
        public bool RemoveFollowing(DTOFollow dto)
        {
            return Followings.Remove(dto.UserId.ToString(), dto.FollowingUserId.ToString());
        }

        private static double GetScore(DateTime timestamp)
        {
            const long ticks = 635628074500000000;
            const double dd = 10000000;
            return ((timestamp.Ticks - ticks) / dd) % 10000;//reduces ticks number to xxxx number with unique digits 
        }

        public static ConnectionMultiplexer Connection;
        
        public static ReadSideService GetInstance(string redisconfiguration,int dbnumber)
        {
            if (Connection == null)
            {
                var configurationOptions = ConfigurationOptions.Parse(redisconfiguration);
                //configurationOptions.SyncTimeout = 5000;
                //configurationOptions.AbortOnConnectFail = true;
                Connection = ConnectionMultiplexer.Connect(configurationOptions);
            }
            var db0 = Connection.GetDatabase(dbnumber);
            var srv = new ReadSideService(db0);
            return srv;
        }

        public static void FlushDatabase(ConnectionMultiplexer conn, int dbNumber)
        {
            var endpoints = conn.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                conn.GetServer(endpoint).FlushDatabase(dbNumber);
            }
        }
    }
}
