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
        public IEnumerable<VideoVM> FeaturedVideos;
        public int TotalPages = 0;
        public VideosVM(IVideoRepository Data, int pageNumber = 0)
            : base(Data)
        {
            var videos = DataRepository.GetMostRecentVideos(pageNumber, GlobalInfo.VIDEOSPERPAGE, CurrentUser);

            foreach (var video in videos)
            {
                video.Content.Entry = video.Content.Entry.Substring(0, Math.Min(video.Content.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            var tmpCount = Data.All<Video>().Count();

            FeaturedVideos = videos.Select(video => new VideoVM(video));
            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.VIDEOSPERPAGE);
        }
    }
}