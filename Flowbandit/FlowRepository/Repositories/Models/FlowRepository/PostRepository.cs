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
    public class PostRepository : DataRepository<FlowCollectionEntities>, IPostRepository
    {

        public PostRepository() : base()
        {
        }

        public PostRepository(FlowCollectionEntities context)
            : base(context)
        {
        }

        public List<Post> GetMostRecentPosts(int skip, int take, int? userId = null, bool shouldStripTags = true)
        {
            var baseQuery = All<Post>().Where(p => p.Visible);

            if(null != userId)
            {
                baseQuery = baseQuery.Where(x => x.FK_UserID == userId.Value);
            }

            var posts = GetMostRecentPosts(baseQuery, skip, take);

            if(shouldStripTags)
            {
                posts.ForEach(p => StripTags(p));
            }
            
            return posts;    
        }

        public Post VisiblePostByIDWithCommentsTagsUsers(int id)
        {
            return AllIncluding<Post>(p => p.PostComments, p => p.TagsToPosts, p => p.User).FirstOrDefault(p => p.ID == id && p.Visible);
        }

        protected List<Post> GetMostRecentPosts(IQueryable<Post> baseQuery, int skip, int take)
        {
            return baseQuery.OrderByDescending(p => p.Created)
                                            .ThenByDescending(p => p.ID)
                                            .Skip(skip)
                                            .Take(take)
                                            .ToList();
        }


        public void EditPost(Post post, List<int> tagIds)
        {
            _context.Entry(post).State = EntityState.Modified;

            this.SanitizeEntry(post);

            //TODO Refactor this into a generic method, for this and videos
            var tagsToRemove = _context.TagsToPosts.Where(tp => tp.FK_PostID == post.ID);
            _context.TagsToPosts.RemoveRange(tagsToRemove);

            foreach (var tagId in tagIds)
            {
                post.TagsToPosts.Add(new TagsToPost { FK_PostID = post.ID, FK_TagID = tagId });
            }
        }

        public void Add(Post post)
        {
            this.SanitizeEntry(post);
            base.Add(post);
        }

        //Posting is done raw normally
        protected void SanitizeEntry(Post post, bool isEncoded = false)
        {
            post.Entry = HtmlDisplayRule.SanitizeHtml(post.Entry, isEncoded);
        }

        //Posting is encoded normally
        protected void StripTags(Post post, bool isEncoded = true)
        {
            post.Entry = HtmlDisplayRule.StripTags(post.Entry, isEncoded);
        }
    }
}
