using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class NotificationRepository : DataRepository<FlowCollectionEntities>, INotificationRepository
    {
        public List<Notification> GetAllNotifications(int userId)
        {
            return this.GetUserNotificationQuery(userId).OrderBy(x => x.Timestamp).ToList();
        }

        public List<Notification> GetUnreadNotifications(int userId)
        {
            return this.GetUserNotificationQuery(userId)
                            .Where(x => x.IsViewed == false)
                            .OrderBy(x => x.Timestamp)
                            .ToList();


        }

        protected IQueryable<Notification> GetUserNotificationQuery(int userId)
        {
            return this._context.Notifications.Where(x => x.ReceiverUserId == userId);
        }
    }

}
