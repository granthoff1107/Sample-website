using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flowbandit.Models
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
