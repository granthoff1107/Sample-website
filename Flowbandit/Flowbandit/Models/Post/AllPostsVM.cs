using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flowbandit;
using FlowRepository;
using Flowbandit.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;

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

            Posts = Data.GetMostRecentPosts(PageNumber * GlobalInfo.RESULTSPERPAGE, GlobalInfo.RESULTSPERPAGE);

            //TODO: Refactor this and VideosVM 
            //Sanitize the posts for the view
            foreach(var post in Posts)
            {
                post.Entry = string.Concat(HtmlDisplayRule.GetSanitizedText(post.Entry, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH) + "...");
            }

            var tmpCount = Data.All<Post>().Count();

            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.RESULTSPERPAGE);
                
        }
    }
}