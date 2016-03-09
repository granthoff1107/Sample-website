using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface ITagRepository : IFlowRepository
    {
        List<Tag> TagsStartingWith(string term);
        List<Post> SearchTagsForPosts(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0);
        List<Video> SearchTagsForVideos(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0);
    }
}
