using FlowRepository;
using FlowService.DTOs.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Videos
{
    public class VideoDTO : ContentBaseModel<Video>
    {
        #region Constructors

        public VideoDTO(Video video) : base(video)
        {
        }

        #endregion

        public int VideoId
        {
            get
            {
                return _parentContent != null ? _parentContent.Id : default(int);
            }
        }

        public string VideoUrl
        {
            get
            {
                return _parentContent != null ? _parentContent.VideoUrl : string.Empty;
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                return _parentContent != null ? _parentContent.ThumbnailUrl : string.Empty;
            }
        }

    }
}
