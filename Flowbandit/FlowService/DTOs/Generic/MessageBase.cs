using FlowRepository;
using FlowRepository.ExtendedModels.Contracts;
using FlowService.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Generic
{
    public class MessageBase<T>
        where T : IHasMessageProperties
    {
        protected T entity;

        public MessageBase(T entity)
        {
            this.entity = entity;
        }

        public int Id
        {
            get { return this.entity.Id; }
        }

        public string Data
        {
            get { return this.entity.Data; }
        }

        public int ReceiverUserId
        {
            get { return this.entity.ReceiverUserId; }
        }

        public UserDTO ReceiverUser
        {
            get { return new UserDTO(this.entity.User); }
        }

        public bool IsViewed
        {
            get { return this.entity.IsViewed; }
        }

        public DateTime Timestamp
        {
            get { return this.entity.Timestamp; }
        }
    }
}
