using FlowRepository;
using FlowService.DTOs.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowRepository.Models.Pagination;

namespace FlowService.DTOs.Videos
{
    public class VideosDTO : ContentsBaseModel<Video, VideoDTO>
    {
        #region Constructors

        public VideosDTO(IEnumerable<Video> entities, PaginationInfo pageInfo) : base(entities, pageInfo)
        {
        }

        #endregion

        protected override Func<Video, VideoDTO> _createEntity
        {
            get
            {
                return x => new VideoDTO(x);
            }
        }
    }
}
