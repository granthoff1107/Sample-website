using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Generic
{
    public interface IComment
    {
        string Username { get; }         
        bool AnonUser { get; }
        string CommentText { get; }
        string Created { get; }
        string UserProfilePicUrl { get; }
        bool hasChildren { get; }
        List<IComment> Children { get; }
    }
}
