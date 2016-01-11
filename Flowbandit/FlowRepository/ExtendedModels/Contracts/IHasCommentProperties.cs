using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExtendedModels.Contracts
{
    public interface IHasCommentProperties
    {
        Nullable<int> FK_UserID { get; set; }
        System.DateTime Created { get; set; }
    }
}
