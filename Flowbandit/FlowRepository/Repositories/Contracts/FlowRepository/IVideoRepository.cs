using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IVideoRepository : IContentRepository
    {
        List<Video> GetMostRecentVideos(int pageNumber, int resultsPerPage, int currentUser = 0, int? userId = null, bool shouldStripTags = true);

        Video FindVisibleVideoWithCommentsTagsUser(int id,  int currentUser = 0);

        void EditVideo(Video video, List<int> tagIds);

        List<Video> SearchTitles(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0);
    }
}
