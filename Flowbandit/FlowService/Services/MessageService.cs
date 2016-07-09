using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using FlowService.DTOs.Messages;
using FlowService.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowRepository;
using FlowService.Services.Contracts;

namespace FlowService.Services
{
    //TODO Make this an actual service
    public class MessageService : ServiceBase<IMessageRepository>, IMessageService
    {
        public MessageService(IMessageRepository repository) : base(repository)
        {
        }

        public MessageDTO[] GetUnreadMessages(int userId)
        {
            var messages = this._repository.GetUnreadMessages(userId);
            return messages.Select(m => new MessageDTO(m)).ToArray();
        }

        public MessageDTO[] GetConversationMessages(int userId, int participantId)
        {
            var messages = this._repository.GetUserConversation(userId, participantId);
            return messages.Select(m => new MessageDTO(m)).ToArray();
        }

        //TODO: currentUserId should be grabbed from current context thread 
        //however since this should be a service, this should be set inside of the context
        // using custom behaviors.  For the time being we will just pass in the currentUserId
        public void AddMessage(Message message, int currentUserId)
        {
            message.Timestamp = DateTime.Now;
            message.IsViewed = false;
            message.SenderUserId = currentUserId;
            this._repository.Add(message);
            this._repository.SaveChanges();
        }
    }
}
