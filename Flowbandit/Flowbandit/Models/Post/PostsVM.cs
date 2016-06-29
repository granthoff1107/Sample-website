using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flowbandit;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Data.Rules;
using Flowbandit.Models.Generic;
using FlowRepository.Models.Pagination;
using FlowService.DTOs.Generic;

namespace Flowbandit.Models
{
    public class PostsVM : BaseModel
    {
        public PostsVM(List<Post> posts, PaginationInfo pageInfo)
        {
            FeaturedPosts = posts.Select(x => new PostVM(x)).ToList();
            PageNumber = pageInfo.PageNumber;
            TotalPages = pageInfo.TotalPages;
        }

        public IEnumerable<PostVM> FeaturedPosts { get; set; }
        public int PageNumber = 0;
        public int TotalPages = 0;
        //GlobalInfo.RESULTSPERPAGE

    }
}