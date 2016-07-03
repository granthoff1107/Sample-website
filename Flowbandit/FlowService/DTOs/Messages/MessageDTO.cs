using FlowRepository;
using FlowService.DTOs.Generic;
using FlowService.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Messages
{
    public class MessageDTO : MessageBase<Message>
    {
        public MessageDTO(Message message) : base(message)
        { 
        }

        public int SenderUserId 
        {
            get{ return this.entity.SenderUserId; }
        }

        public UserDTO SenderUser
        {
            get { return new UserDTO(this.entity.User1); }
        }
    }
}
