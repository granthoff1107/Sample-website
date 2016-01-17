using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IVideoRepository : IFlowRepository
    {
        List<Video> GetMostRecentVideos(int PageNumber, int ResultsPerPage);

        Video FindVisibleVideoWithCommentsTagsUser(int ID);
    }
}
