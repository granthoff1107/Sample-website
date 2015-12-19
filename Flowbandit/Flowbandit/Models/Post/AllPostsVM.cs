using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flowbandit;
using FlowRepository;

namespace Flowbandit.Models
{
    public class AllPostsVM : BaseModel
    {
       
        public int PageNumber = 0;
        public List<Post> Posts = new List<Post>();
        public int TotalPages = 0;
        public AllPostsVM(IPostRepository Data, int PageNumber = 0, int TagID = 0)
            : base(Data)
        {
            this.PageNumber = PageNumber;

            Posts = Data.VisiblePostsByCreatedDate(PageNumber * GlobalInfo.RESULTSPERPAGE, GlobalInfo.RESULTSPERPAGE);

            var tmpCount = Data.All<Post>().Count();

            TotalPages = this.GetTotalPageCountFromItems(tmpCount, GlobalInfo.RESULTSPERPAGE);
                
        }
    }
}