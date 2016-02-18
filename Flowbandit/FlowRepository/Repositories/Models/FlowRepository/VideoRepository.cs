using FlowRepository.Data.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class VideoRepository : DataRepository<FlowCollectionEntities>, IVideoRepository
    {
        public VideoRepository() : base()
        {
        }

        public VideoRepository(FlowCollectionEntities context) : base (context)
        {
        }

        public List<Video> GetMostRecentVideos(int skip, int take, int? userId = null, bool shouldStripTags = true)
        {
            var baseQuery = All<Video>().Where(p => p.Visible);

            if(null != userId)
            {
                baseQuery = baseQuery.Where(x => x.FK_UserID == userId.Value);
            }

            var videos = GetMostRecentVideos(baseQuery, skip, take);

            if(shouldStripTags)
            {
                videos.ForEach(v => StripTags(v, false));
            }

            return videos;

        }

        public Video FindVisibleVideoWithCommentsTagsUser(int id)
        {
            return AllIncluding<Video>(p => p.VideoComments, p => p.TagsToVideos, p => p.User).FirstOrDefault(p => p.ID == id && p.Visible);
        }

        protected List<Video> GetMostRecentVideos(IQueryable<Video> baseQuery, int skip, int take)
        {
            return baseQuery.OrderByDescending(p => p.Created)
                                            .ThenByDescending(p => p.ID)
                                            .Skip(skip)
                                            .Take(take)
                                            .ToList();
        }

        public void Add(Video video)
        {
            this.SanitizeDescription(video);
            base.Add(video);
        }

        public void EditVideo(Video video, List<int> tagIds)
        {
            _context.Entry(video).State = EntityState.Modified;

            this.SanitizeDescription(video);

            //TODO Refactor this into a generic method, for this and posts
            var tagsToRemove = _context.TagsToVideos.Where(tv => tv.FK_VideoID == video.ID);
            _context.TagsToVideos.RemoveRange(tagsToRemove);

            foreach (var tagId in tagIds)
            {
                video.TagsToVideos.Add(new TagsToVideo { FK_VideoID = video.ID, FK_TagID = tagId });
            }
        }

        //Posting is done raw normally
        protected void SanitizeDescription(Video video, bool isEncoded = false)
        {
            video.Description = HtmlDisplayRule.SanitizeHtml(video.Description, isEncoded);
        }

        //Posting is encoded normally
        protected void StripTags(Video video, bool isEncoded = true)
        {
            video.Description = HtmlDisplayRule.StripTags(video.Description, isEncoded);
        }
    }
}
