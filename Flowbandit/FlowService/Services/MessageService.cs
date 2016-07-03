using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using FlowService.DTOs.Messages;
using FlowService.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services
{
    //TODO Make this an actual service
    public class MessageService : ServiceBase<IMessageRepository>
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
    }
}
