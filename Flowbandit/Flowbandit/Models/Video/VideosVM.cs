using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideosVM : BaseModel<IVideoRepository>
    {
        public List<Video> FeaturedVideos;
        public int TotalPages = 0;
        public VideosVM(IVideoRepository Data, int pageNumber = 0)
            : base(Data)
        {
            FeaturedVideos = DataRepository.GetMostRecentVideos(pageNumber, GlobalInfo.VIDEOSPERPAGE);

            foreach (var video in FeaturedVideos)
            {
                video.Description = video.Description.Substring(0, Math.Min(video.Description.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            var tmpCount = Data.All<Video>().Count();

            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.VIDEOSPERPAGE);
        }
    }
}