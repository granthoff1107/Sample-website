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
        public List<Post> VisiblePostsByCreatedDate(int Skip, int Take)
        {
           return All<Post>().Where(p => p.Visible)
                                            .OrderByDescending(p => p.Created)
                                            .ThenByDescending(p => p.ID)
                                            .Skip(Skip)
                                            .Take(Take)
                                            .ToList();
        }

        public Post VisiblePostByIDWithCommentsTagsUsers(int ID)
        {
            return AllIncluding<Post>(p => p.PostComments, p => p.TagsToPosts, p => p.User).FirstOrDefault(p => p.ID == ID && p.Visible);
        }
    }
}
