using FlowRepository;
using FlowService.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Friends
{
    public class FriendDTO
    {
        protected int currentUserId;
        protected Friend entity;

        #region Constructor
        
        public FriendDTO(Friend friend, int currentUserId)
        {
            this.entity = friend;
            this.currentUserId = currentUserId;
        }

        #endregion

        public int Id
        {
            get { return this.entity.Id; }
        }

        public int FriendUserId
        {
            get {
                return this.FriendIsCurrentUser ? this.entity.UserId : this.entity.FriendUserId; }
        }

        public int UserId
        {
            get { return this.FriendIsCurrentUser ? this.entity.FriendUserId : this.entity.UserId; }
        }

        public DateTime Timestamp
        {
            get
            {
                return this.entity.Timestamp;
            }
        }

        public UserDTO FriendUser
        {
            get
            {
                var user = this.FriendIsCurrentUser ? this.entity.User : this.entity.User1; 
                return new UserDTO(user);
            }
        }

        protected bool FriendIsCurrentUser
        {
            get
            {
                return this.entity.FriendUserId == currentUserId;
            }
        }
    }
}
