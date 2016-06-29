using FlowRepository.Data.Rules;
using FlowRepository.ExtendedModels.Contracts;
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
    public class ContentRepository : DataRepository<FlowCollectionEntities>, IContentRepository
    {
        #region Constructors

        public ContentRepository()
            : base()
        {
        }

        public ContentRepository(FlowCollectionEntities context)
            : base(context)
        {
        }
 
        #endregion

        #region Public Methods

        public override void Add<T>(T entity)
        {
            //HACK: Inheritiance will not allow overriding with additional constraints
            if(entity is IHasContent)
            {
                var content = (entity as IHasContent).Content;
                content.Created = DateTime.Now;
            }

            base.Add<T>(entity);
        }

        public void Edit<T>(T entity, List<int> tagIds)
            where T : class, IHasContent
        {
            this.SetModified(entity);
            this.SetModified(entity.Content);

            var tagsToRemove = _context.TagsToContents.Where(tp => tp.ContentId == entity.Content.Id);
            _context.TagsToContents.RemoveRange(tagsToRemove);

            foreach (var tagId in tagIds)
            {
                entity.Content.TagsToContents.Add(new TagsToContent { ContentId = entity.Content.Id, TagId = tagId });
            }
        }

        public List<T> SearchContent<T>(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0)
            where T : class, IHasContent
        {
            var baseQuery = this.GetAllVisibleBaseQuery(All<T>(), currentUser);

            foreach (var term in searchTerms)
            {
                baseQuery = baseQuery.Where(x => x.Content.Title.Contains(term));
            }

            return this.GetMostRecentVisibleContent(baseQuery, pageNumber, resultsPerPage);
        }

        public List<T> GetMostRecentVisibleContent<T>(IQueryable<T> baseQuery, int pageNumber, int resultsPerPage, out int count, int currentUser = 0, int? filterUserId = null, bool shouldStripTags = true)
            where T : class, IHasContent
        {
            baseQuery = this.GetAllVisibleBaseQuery(baseQuery, currentUser);

            if (null != filterUserId)
            {
                baseQuery = baseQuery.Where(x => x.Content.UserId == filterUserId.Value);
            }

            count = baseQuery.Count();

            var contents = baseQuery.OrderByDescending(p => p.Content.Created)
                                            .ThenByDescending(p => p.Content.Id)
                                            .Skip(pageNumber * resultsPerPage)
                                            .Take(resultsPerPage)
                                            .ToList();

            //TODO this should happen externally in a service
            if (shouldStripTags)
            {
                contents.ForEach(p => StripTags(p.Content));
            }

            return contents;
        }

        public List<T> GetMostRecentVisibleContent<T>(IQueryable<T> baseQuery, int pageNumber, int resultsPerPage, int currentUser = 0, int? filterUserId = null, bool shouldStripTags = true)
            where T : class, IHasContent
        {
            int count;
            return GetMostRecentVisibleContent(baseQuery, pageNumber, resultsPerPage, out count, currentUser, filterUserId, shouldStripTags);
        }

        public T GetVisibleContentByIdWithCommentsTagsUsers<T>(int id, int currentUser = 0)
            where T : class, IHasContent
        {
            return this.GetAllVisibleBaseQuery(AllIncluding<T>(x => x.Content, x => x.Content.TagsToContents, x => x.Content.User), currentUser).FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<T> GetAllVisibleBaseQuery<T>(IQueryable<T> baseQuery, int currentUserId)
    where T : class, IHasContent
        {
            return baseQuery.Where(x => x.Content.Visible || x.Content.UserId == currentUserId);
        }

        public void AddComment(ContentComment comment, int? currentUser = null)
        {
            comment.UserId = currentUser;
            comment.Created = DateTime.Now;
            Add(comment);
        }
        
        #endregion

        #region Protected Methods

        //Content is encoded normally
        protected void StripTags(Content content, bool isEncoded = true)
        {
            content.Entry = HtmlDisplayRule.StripTags(content.Entry, isEncoded);
        }

        #endregion
    }
}
