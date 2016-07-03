using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IMessageRepository : IFlowRepository
    {
        List<Message> GetUnreadMessages(int currentUserId);
        List<Message> GetUserConversation(int currentUserId, int participantUserId );
    }
}
