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
        public VideoRepository() : base()
        {
        }

        public VideoRepository(FlowCollectionEntities context) : base (context)
        {
        }

        public List<Video> GetMostRecentVideos(int skip, int take, int? userId = null)
        {
            var baseQuery = All<Video>().Where(p => p.Visible);

            if(null != userId)
            {
                baseQuery = baseQuery.Where(x => x.FK_UserID == userId.Value);
            }
            return GetMostRecentQueryable(baseQuery, skip, take);
        }

        public Video FindVisibleVideoWithCommentsTagsUser(int id)
        {
            return AllIncluding<Video>(p => p.VideoComments, p => p.TagsToVideos, p => p.User).FirstOrDefault(p => p.ID == id && p.Visible);
        }

        protected List<Video> GetMostRecentQueryable(IQueryable<Video> baseQuery, int skip, int take)
        {
            return baseQuery.OrderByDescending(p => p.Created)
                                            .ThenByDescending(p => p.ID)
                                            .Skip(skip)
                                            .Take(take)
                                            .ToList();
        }
    }
}
