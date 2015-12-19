using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{

    public class VideoRepository : DataRepository, IVideoRepository
    {
        public Dictionary<string, List<Video>> GetFeaturedVideos(int PageNumber, int ResultsPerPage)
        {
            return All<TagsToVideo>().OrderByDescending(t => t.Video.Created)
                                    .Skip(PageNumber * ResultsPerPage)
                                    .Take(ResultsPerPage)
                                    .GroupBy(t => t.Tag).ToDictionary(k => k.Key.Name, v => v.Select(vid => vid.Video).ToList());
            //return All<TagsToVideo>().GroupBy(t => t.Tag)
            //                        .OrderByDescending(t => t.Select(v => v.Video.Created).FirstOrDefault())
            //                        .Select(v => new {key = v.Key.Name,  values = v.Take(VideosPerRow).Select(v1 => v1.Video).ToList() } )
            //                        .Take(RowsPerPage).ToDictionary(k => k.key, v => v.values);
        }


        public Video FindVisibleVideoWithCommentsTagsUser(int ID)
        {
            return AllIncluding<Video>(p => p.VideoComments, p => p.TagsToVideos, p => p.User).FirstOrDefault(p => p.ID == ID && p.Visible);

        }

        //this should be refactored into its own repo with the one that exists in posts 
        public List<Tag> TagsStartingWith(string term)
        {
            return All<Tag>().Where(t => t.Name.ToLower().StartsWith(term)).ToList();
        }
    }
}
