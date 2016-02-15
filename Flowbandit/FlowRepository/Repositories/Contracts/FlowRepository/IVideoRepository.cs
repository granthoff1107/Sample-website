using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IVideoRepository : IFlowRepository
    {
        List<Video> GetMostRecentVideos(int skip, int take, int? userId = null);

        Video FindVisibleVideoWithCommentsTagsUser(int id);
    }
}
