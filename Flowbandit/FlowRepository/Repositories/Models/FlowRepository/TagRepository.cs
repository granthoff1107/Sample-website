using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class TagRepository : DataRepository<FlowCollectionEntities>, ITagRepository
    {
        #region Constructors

        public TagRepository()
            : base()
        {
        }

        public TagRepository(FlowCollectionEntities context)
            : base(context)
        {
        }

        #endregion

        #region ITagRepository Methods

        public List<Tag> TagsStartingWith(string term)
        {
            return All<Tag>().Where(t => t.Name.ToLower().StartsWith(term)).ToList();
        }

        public List<Post> SearchTagsForPosts(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUserId = 0)
        {
            var baseQuery = this.GetBaseSearchTagQueryable(searchTerms);

            var postQuery = baseQuery.SelectMany(tag => tag.TagsToContents.SelectMany(tagToContent => tagToContent.Content.Posts));    
            var postRepo = this.ConvertToRepository<PostRepository>();
            postQuery = postRepo.GetAllVisibleBaseQuery(postQuery, currentUserId);
            return postRepo.GetMostRecentVisibleContent(postQuery, pageNumber, resultsPerPage, currentUserId);
        }

        public List<Video> SearchTagsForVideos(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUserId = 0)
        {
            var baseQuery = this.GetBaseSearchTagQueryable(searchTerms);

            var videoQuery = baseQuery.SelectMany(tag => tag.TagsToContents.SelectMany(tagToContent => tagToContent.Content.Videos));
            var videoRepo = this.ConvertToRepository<VideoRepository>();
            videoQuery = videoRepo.GetAllVisibleBaseQuery(videoQuery, currentUserId);
            return videoRepo.GetMostRecentVisibleContent(videoQuery, pageNumber, resultsPerPage, currentUserId);
        } 

        #endregion

        #region Protected Methods

        protected IQueryable<Tag> GetBaseSearchTagQueryable(string[] searchTerms)
        {
            var baseQuery = All<Tag>();
            foreach (var term in searchTerms)
            {
                baseQuery = baseQuery.Where(x => x.Name.Contains(term));
            }
            return baseQuery;
        }

        #endregion
    }
}
