using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface INotificationRepository : IFlowRepository
    {
        List<Notification> GetUnreadNotifications(int userId);
        List<Notification> GetAllNotifications(int userId);
    }
}
