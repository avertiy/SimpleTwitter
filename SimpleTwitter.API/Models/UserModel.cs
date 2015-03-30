using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Security;
using SimpleTwitter.ReadSide.Data.DTOs;

namespace SimpleTwitter.API.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        [Required]
        public string UserName { get; set; }


        public IEnumerable<KeyValuePair<string,string>> Followers { get; set; }
        public IEnumerable<KeyValuePair<string, string>> Followings { get; set; }

        public UserModel()
        {
        }

        public UserModel(DTOUser dto)
        {
            Id = dto.Id;
            UserName = dto.UserName;
        }
    }
    public class PostModel
    {
        public PostModel()
        {
        }

        public PostModel(DTOPost dto)
        {
            Id = dto.Id;
            UserId = dto.UserId;
            UserName = dto.UserName;
            MessageText = dto.Text;
            TimeStamp = dto.TimeStamp;
        }

        public Guid Id { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        [Required]
        [MaxLength(140)]
        public string MessageText { get; set; }
        public DateTime TimeStamp { get; set; }
    }
    public class GetFeedRequestModel
    {
        private long _stop =10;
        public Guid? UserId { get; set; }
        
        public long Start { get; set; }

        public long Stop
        {
            get { return _stop; }
            set { _stop = value; }
        }
    }
    public class FollowRequestModel
    {
        public Guid UserId { get; set; }
        public Guid FollowingId { get; set; }
    }
}