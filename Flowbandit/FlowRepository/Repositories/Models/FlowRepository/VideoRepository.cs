using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class VideoRepository : DataRepository<FlowCollectionEntities>, IVideoRepository
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
    }
}
