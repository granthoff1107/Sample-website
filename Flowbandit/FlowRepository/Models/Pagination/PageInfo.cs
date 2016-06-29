using FlowRepository.Rules.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Models.Pagination
{
    public class PageTrackingInfo
    {
        public PageTrackingInfo(int resultsPerPage, int pageNumber)
        {
            this.PageNumber = pageNumber;
            this.ResultsPerPage = resultsPerPage;
        }

        public int PageNumber { get; set; }
        public int ResultsPerPage { get; set; }
        

       
    }
}
