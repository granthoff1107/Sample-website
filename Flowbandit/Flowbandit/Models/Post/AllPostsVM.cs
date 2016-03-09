using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flowbandit;
using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Data.Rules;

namespace Flowbandit.Models
{
    public class AllPostsVM : BaseModel<IPostRepository>
    {
        public int PageNumber = 0;
        public IEnumerable<PostVM> FeaturedPosts;
        public int TotalPages = 0;
        public AllPostsVM(IPostRepository Data, int PageNumber = 0, int TagID = 0)
            : base(Data)
        {
            this.PageNumber = PageNumber;

            var posts = Data.GetMostRecentPosts(PageNumber, GlobalInfo.RESULTSPERPAGE, CurrentUser);

            foreach(var post in posts)
            {
                post.Content.Entry = post.Content.Entry.Substring(0, Math.Min(post.Content.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            var tmpCount = Data.All<Post>().Count();

            FeaturedPosts = posts.Select(post => new PostVM(post));


            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.RESULTSPERPAGE);
                
        }
    }
}