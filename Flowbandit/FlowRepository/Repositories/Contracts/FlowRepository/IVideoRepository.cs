using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IVideoRepository : IFlowRepository
    {
        List<Video> GetMostRecentVideos(int skip, int take, int? userId = null, bool shouldStripTags = true);

        Video FindVisibleVideoWithCommentsTagsUser(int id);

        void Add(Video Video);

        void EditVideo(Video video, List<int> tagIds);
    }
}
