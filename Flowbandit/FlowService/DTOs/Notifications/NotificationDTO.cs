using FlowRepository;
using FlowService.DTOs.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Notifications
{
    public class NotificationDTO : MessageBase<Notification>
    {
        public NotificationDTO(Notification notification) : base(notification)
        {
        }

        public string Url
        {
            get { return this.entity.Url; }
        }
    }
}
