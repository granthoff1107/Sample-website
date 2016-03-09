using Flowbandit.Models.Generic;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideoVM : ContentBaseModel<IVideoRepository, Video>
    {
        public int VideoId
        {
            get
            {
                return _contentParent != null ? _contentParent.Id : default(int);
            }
        }

        public string VideoUrl
        {
            get
            {
                return _contentParent != null ? _contentParent.VideoUrl : string.Empty;
            }
        }

        public string ThumbnailUrl
        {
            get
            {
                return _contentParent != null ? _contentParent.ThumbnailUrl : string.Empty;
            }
        }

        public VideoVM(IVideoRepository videoRepository, int id)
            : base(videoRepository, id)
        {
            
        }

        public VideoVM()
            : base()
        {
           
        }

        public VideoVM(Video video)
            : base(video)
        {
           
        }
    }

}