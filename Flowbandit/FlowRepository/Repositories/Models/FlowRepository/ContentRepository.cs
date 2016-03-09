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

        public override void Add<T>(T Entity)
        {
            //HACK: Inheritiance will not allow overriding with additional constriants
            if(Entity is IHasContent)
            { 
                this.SanitizeEntry((Entity as IHasContent).Content);
            }
            base.Add<T>(Entity);
        }

        public void Edit<T>(T Entity, List<int> tagIds)
            where T : class, IHasContent
        {
            this.SanitizeEntry(Entity.Content);
            _context.Entry(Entity).State = EntityState.Modified;
            _context.Entry(Entity.Content).State = EntityState.Modified;

            //TODO Refactor this into a generic method, for this and videos
            var tagsToRemove = _context.TagsToContents.Where(tp => tp.ContentId == Entity.Content.Id);
            _context.TagsToContents.RemoveRange(tagsToRemove);

            foreach (var tagId in tagIds)
            {
                Entity.Content.TagsToContents.Add(new TagsToContent { ContentId = Entity.Content.Id, TagId = tagId });
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

        public List<T> GetMostRecentVisibleContent<T>(IQueryable<T> baseQuery, int pageNumber, int resultsPerPage, int currentUser = 0, int? filterUserId = null, bool shouldStripTags = true)
            where T : class, IHasContent
        {
            baseQuery = this.GetAllVisibleBaseQuery(baseQuery, currentUser);

            if (null != filterUserId)
            {
                baseQuery = baseQuery.Where(x => x.Content.UserId == filterUserId.Value);
            }

            var contents = baseQuery.OrderByDescending(p => p.Content.Created)
                                            .ThenByDescending(p => p.Content.Id)
                                            .Skip(pageNumber * resultsPerPage)
                                            .Take(resultsPerPage)
                                            .ToList();
            if (shouldStripTags)
            {
                contents.ForEach(p => StripTags(p.Content));
            }

            return contents;
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

        #endregion
    }
}
