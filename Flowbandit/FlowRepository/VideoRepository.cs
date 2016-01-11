using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{
    public class VideoRepository : DataRepository, IVideoRepository
    {
        public List<Video> GetMostRecentVideos(int pageNumber, int resultsPerPage)
        {
            return All<Video>().OrderByDescending(v => v.Created)
                               .ThenByDescending(v => v.ID)
                               .Skip(pageNumber * resultsPerPage)
                               .Take(resultsPerPage).ToList();
        }

        public Video FindVisibleVideoWithCommentsTagsUser(int id)
        {
            return AllIncluding<Video>(p => p.VideoComments, p => p.TagsToVideos, p => p.User).FirstOrDefault(p => p.ID == id && p.Visible);
        }

        //this should be refactored into its own repo with the one that exists in posts 
        public List<Tag> TagsStartingWith(string term)
        {
            return All<Tag>().Where(t => t.Name.ToLower().StartsWith(term)).ToList();
        }
    }
}
