using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IPostRepository : IFlowRepository
    {
        List<Post> VisiblePostsByCreatedDate(int Skip, int Take);

        Post VisiblePostByIDWithCommentsTagsUsers(int ID);
    }
}
