using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExtendedModels.Contracts
{
    public interface IHasMessageProperties
    {
        int Id { get; set; }
        string Data { get; set; }
        int ReceiverUserId { get; set; }
        User User { get; set; }
        bool IsViewed { get; set; }
        DateTime Timestamp { get; set; }
    }
}
