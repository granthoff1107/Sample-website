using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{
    public class PostRepository : DataRepository, IPostRepository
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
