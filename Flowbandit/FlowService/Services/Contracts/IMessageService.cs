using FlowRepository;
using FlowService.DTOs.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services.Contracts
{
    public interface IMessageService
    {
        MessageDTO[] GetUnreadMessages(int userId);
        MessageDTO[] GetConversationMessages(int userId, int participantId);
        void AddMessage(Message message, int currentUserId);

    }
}
