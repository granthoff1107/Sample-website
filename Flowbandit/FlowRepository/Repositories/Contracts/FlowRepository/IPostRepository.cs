using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IPostRepository : IContentRepository
    {
        List<Post> GetMostRecentPosts(int pageNumber, int resultsPerPage, int currentUser = 0, int? userId = null, bool shouldStripTags = true);

        Post VisiblePostByIDWithCommentsTagsUsers(int id, int currentUser = 0);

        void EditPost(Post post, List<int> tagIds);

        List<Post> SearchPostTitles(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0);
    }
}
