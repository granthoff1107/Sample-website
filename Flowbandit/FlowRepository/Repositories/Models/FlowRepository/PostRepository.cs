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
    public class PostRepository : ContentRepository, IPostRepository
    {
        #region Constructors

        public PostRepository()
            : base()
        {
        }

        public PostRepository(FlowCollectionEntities context)
            : base(context)
        {
        }
 
        #endregion

        #region IPostRepository Members

        public List<Post> GetMostRecentPosts(int pageNumber, int resultsPerPage, int currentUser = 0, int? userId = null, bool shouldStripTags = true)
        {
            return this.GetMostRecentVisibleContent(All<Post>(), pageNumber, resultsPerPage, currentUser, userId, shouldStripTags);
        }

        public Post VisiblePostByIDWithCommentsTagsUsers(int id, int currentUser = 0)
        {
            return GetVisibleContentByIdWithCommentsTagsUsers<Post>(id, currentUser);
        }

        public void EditPost(Post post, List<int> tagIds)
        {
            this.Edit<Post>(post, tagIds);
        }

        public List<Post> SearchPostTitles(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0)
        {
            return this.SearchContent<Post>(searchTerms, pageNumber, resultsPerPage, currentUser);
        }

        #endregion        

    }
}
