using FlowRepository;
using FlowRepository.Data.Rules;
using FlowRepository.ExtendedModels.Contracts;
using FlowRepository.Models.Pagination;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Rules.Pagination;
using FlowService.DTOs.Posts;
using FlowService.DTOs.Videos;
using FlowService.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services
{
    public class ContentService<TRepository, TEntity> : ServiceBase<TRepository>
        where TRepository : class, IFlowRepository, IContentRepository
        where TEntity : class, IHasContent
    {

        public ContentService(TRepository repository) : base(repository)
        {

        }

        public void ProcessContent(Content content)
        {
            content.LastModified = DateTime.Now;
            this.SanitizeEntry(content);
        }

        public PaginationInfo GetPaginationInfo(IQueryable<TEntity> baseQuery, PageTrackingInfo trackingInfo)
        {
            var count = baseQuery.Count();
            return new PaginationInfo(count, trackingInfo.ResultsPerPage, trackingInfo.PageNumber);  
        }

        #region Refactor Maybe Abstract Factor

        public VideosDTO GetVideos(PageTrackingInfo pageInfo)
        {
            PaginationInfo paginationInfo;
            //TODO Pass in current user id
            List<Video> videos = this.GetEntriesForDisplay<Video>(pageInfo, out paginationInfo, 0, 500);

            return new VideosDTO(videos, paginationInfo);
        }

        public VideoDTO GetVideo(int id, int userId = 0)
        {
            var video = this._repository.GetVisibleContentByIdWithCommentsTagsUsers<Video>(id, userId);
            return new VideoDTO(video);
        }

        public PostsDTO GetPosts(PageTrackingInfo pageInfo)
        {
            PaginationInfo paginationInfo;
            List<Post> posts = this.GetEntriesForDisplay<Post>(pageInfo, out paginationInfo, 0, 500);

            return new PostsDTO(posts, paginationInfo);
        }

        public PostDTO GetPost(int id, int userId = 0)
        {
            var post = this._repository.GetVisibleContentByIdWithCommentsTagsUsers<Post>(id, userId);
            return new PostDTO(post);
        }

        #endregion

        //TODO Refactor this to be generic
        public List<TContent> GetEntriesForDisplay<TContent>(PageTrackingInfo pageInfo, out PaginationInfo paginationInfo,  int currentUser = 0, int maxLength = 500)
            where TContent : class, IHasContent
        {
            int count;
            var parentContents = _repository.GetMostRecentVisibleContent(_repository.All<TContent>(),
                pageInfo.PageNumber, pageInfo.ResultsPerPage, out count, currentUser);

            paginationInfo = new PaginationInfo(count, pageInfo.ResultsPerPage, pageInfo.PageNumber);

            foreach (var parentContent in parentContents)
            {
                this.StripTags(parentContent.Content);
                var entry = parentContent.Content.Entry;
                parentContent.Content.Entry = entry.Substring(0, Math.Min(entry.Length, maxLength)) + "...";
            }

            return parentContents;
        }

        //Content is done raw normally
        protected void SanitizeEntry(Content content, bool isEncoded = false)
        {
            content.Entry = HtmlDisplayRule.SanitizeHtml(content.Entry, isEncoded);
        }

        //Content is encoded normally
        protected void StripTags(Content content, bool isEncoded = true)
        {
            content.Entry = HtmlDisplayRule.StripTags(content.Entry, isEncoded);
        }
    }
}
