using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.DTOs.Notifications;
using FlowService.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services
{
    //TODO Make this an actual service
    public class NotificationService : ServiceBase<INotificationRepository>
    {
        #region Constructors

        public NotificationService(INotificationRepository repository) : base(repository)
        {
        }

        #endregion

        public NotificationDTO[] GetUnreadNotification(int userId)
        {
            var messages = this._repository.GetUnreadNotifications(userId);
            return messages.Select(m => new NotificationDTO(m)).ToArray();
        }
    }
}
