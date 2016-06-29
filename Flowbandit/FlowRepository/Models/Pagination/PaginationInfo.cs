using FlowRepository.Rules.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Models.Pagination
{
    public class PaginationInfo : PageTrackingInfo
    {
        protected PaginationRule _pageRule = new PaginationRule();

        public PaginationInfo(int totalResults, int resultsPerPage, int pageNumber) 
            : base (resultsPerPage, pageNumber)
        {
            this.TotalResults = totalResults;
        }

        public int TotalResults { get; set; }

        public int TotalPages
        {
            get
            {
                return _pageRule.GetTotalPageCountFromItems(TotalResults, ResultsPerPage);
            }
        }
    }
}
