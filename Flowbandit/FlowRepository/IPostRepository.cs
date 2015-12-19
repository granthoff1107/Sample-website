using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{
    public interface IPostRepository : IRepository
    {
        List<Post> VisiblePostsByCreatedDate(int Skip, int Take);

        List<Tag> TagsStartingWith(string term);

        Post VisiblePostByIDWithCommentsTagsUsers(int ID);


    }
}
