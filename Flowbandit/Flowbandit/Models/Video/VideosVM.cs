using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class VideosVM : BaseModel
    {
        public List<Video> FeaturedVideos;
        public int TotalPages = 0;
        public VideosVM(IVideoRepository Data, int pageNumber = 0)
            : base(Data)
        {
            FeaturedVideos = Data.All<Video>().OrderByDescending(v => v.Created).ThenByDescending(v => v.ID).Skip(pageNumber * GlobalInfo.VIDEOSPERPAGE).Take(GlobalInfo.VIDEOSPERPAGE).ToList();
            var tmpCount = Data.All<Video>().Count();

            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.VIDEOSPERPAGE);

        }
    }
}