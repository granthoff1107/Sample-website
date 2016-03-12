using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flowbandit;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Data.Rules;
using Flowbandit.Models.Generic;

namespace Flowbandit.Models
{
    public class PostsVM : ContentsBaseModel<IPostRepository, Post>
    {
        public IEnumerable<PostVM> FeaturedPosts;
        public PostsVM(IPostRepository repository, int pageNumber = 0)
            : base(repository, pageNumber, GlobalInfo.RESULTSPERPAGE)
        {
            FeaturedPosts = sanitizedEntities.Select(post => new PostVM(post));    
        }
    }
}