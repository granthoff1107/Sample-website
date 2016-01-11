using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{
    public interface IVideoRepository : IRepository
    {
        List<Video> GetMostRecentVideos(int PageNumber, int ResultsPerPage);

        Video FindVisibleVideoWithCommentsTagsUser(int ID);

        List<Tag> TagsStartingWith(string term);
    }
}
