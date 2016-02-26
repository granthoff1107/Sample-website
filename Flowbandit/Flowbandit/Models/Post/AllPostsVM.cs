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
        public List<Post> Posts = new List<Post>();
        public int TotalPages = 0;
        public AllPostsVM(IPostRepository Data, int PageNumber = 0, int TagID = 0)
            : base(Data)
        {
            this.PageNumber = PageNumber;

            Posts = Data.GetMostRecentPosts(PageNumber, GlobalInfo.RESULTSPERPAGE);

            foreach(var post in Posts)
            {
                post.Entry = post.Entry.Substring(0, Math.Min(post.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            var tmpCount = Data.All<Post>().Count();

            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.RESULTSPERPAGE);
                
        }
    }
}