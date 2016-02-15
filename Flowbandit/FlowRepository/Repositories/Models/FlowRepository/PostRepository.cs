using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
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

        public List<Post> GetMostRecentPosts(int skip, int take, int? userId = null)
        {
            var baseQuery = All<Post>().Where(p => p.Visible);

            if(null != userId)
            {
                baseQuery = baseQuery.Where(x => x.FK_UserID == userId.Value);
            }

            return GetMostRecentQueryable(baseQuery, skip, take);
                                            
        }

        public Post VisiblePostByIDWithCommentsTagsUsers(int id)
        {
            return AllIncluding<Post>(p => p.PostComments, p => p.TagsToPosts, p => p.User).FirstOrDefault(p => p.ID == id && p.Visible);
        }

        protected List<Post> GetMostRecentQueryable(IQueryable<Post> baseQuery, int skip, int take)
        {
            return baseQuery.OrderByDescending(p => p.Created)
                                            .ThenByDescending(p => p.ID)
                                            .Skip(skip)
                                            .Take(take)
                                            .ToList();
        }
    }
}
