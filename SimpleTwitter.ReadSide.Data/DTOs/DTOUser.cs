using System;
using System.Collections;
using System.Collections.Generic;
using StackExchange.Redis;

namespace SimpleTwitter.ReadSide.Data.DTOs
{
    [Serializable]
    public class DTOUser
    {
        public string UserName { get; set; }
        public Guid Id { get; set; }
    }
    [Serializable]
    public class DTOPost
    {

        public Guid Id { get; set; }
        /// <summary>
        /// post author
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// post author
        /// </summary>
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    [Serializable]
    public class DTOGetFeedRequest
    {
        private Order _order = Order.Ascending;
        private long _stop =-1;
        public Guid UserId { get; set; }
        public long Start { get; set; }

        public long Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }

        public Order Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
    [Serializable]
    public class DTOFeedResponse
    {
        public Guid? UserId { get; set; }
        public IEnumerable<DTOPost> Feed { get; set; }
    }

    [Serializable]
    public class DTOGetFollowersResponse
    {
        /// <summary>
        /// key: Id value:UserName
        /// </summary>
        public IEnumerable<KeyValuePair<string,string>> Users { get; set; }
    }
    

    [Serializable]
    public class DTOFollow
    {
        public Guid UserId { get; set; }
        public Guid FollowingUserId { get; set; }
        //public DateTime TimeStamp { get; set; }
    }
    [Serializable]
    public class DTOUnFollow
    {
        public Guid UserId { get; set; }
        public Guid UnFollowingUserId { get; set; }
    }
}