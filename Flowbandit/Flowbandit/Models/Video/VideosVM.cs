using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideosVM : ContentsBaseModel<IVideoRepository, Video>
    {
        public IEnumerable<VideoVM> FeaturedVideos;
        public VideosVM(IVideoRepository repository, int pageNumber = 0)
            : base(repository, pageNumber, GlobalInfo.VIDEOSPERPAGE)
        {
            FeaturedVideos = sanitizedEntities.Select(video => new VideoVM(video));
        }
    }
}