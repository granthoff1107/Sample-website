using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class MessageRepository : DataRepository<FlowCollectionEntities>, IMessageRepository
    {
        public List<Message> GetUnreadMessages(int userId)
        {
            return this.GetMessagesWithUsers().Where(x => x.ReceiverUserId == userId)
                        .Where(x => false == x.IsViewed).OrderByDescending(x => x.Timestamp)
                        .ToList();
        }

        public List<Message> GetUserConversation(int currentUserId, int participantUserId)
        {
            //TODO: Restructure the data base so there is a table that contains conversations
            // Then just have the user suscribe to a conversation, that way you can 1 grab converstations easier
            // and 2 then you can have more than 2 people in a conversation
            return this.GetMessagesWithUsers()
                .Where(x => (x.ReceiverUserId == participantUserId && x.SenderUserId == currentUserId)
                             || (x.ReceiverUserId == currentUserId && x.SenderUserId == participantUserId))
                .OrderByDescending(x => x.Timestamp)
                .ToList();
        }

        protected IQueryable<Message> GetMessagesWithUsers()
        {
            return this._context.Messages.Include(x => x.User).Include(x => x.User1);
        }
    }
}
