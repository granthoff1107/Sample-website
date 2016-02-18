using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IPostRepository : IFlowRepository
    {
        List<Post> GetMostRecentPosts(int skip, int take, int? userId = null, bool shouldStripTags = true);

        Post VisiblePostByIDWithCommentsTagsUsers(int id);

        void Add(Post post);

        void EditPost(Post post, List<int> tagIds);
    }
}
