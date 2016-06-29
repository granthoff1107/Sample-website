using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Rules.Pagination
{
    public class PaginationRule
    {
        public int GetTotalPageCountFromItems(int tmpCount, int resultsPerPage)
        {
            int totalPages = 0;
            //make sure there are results and avoid divide by 0
            if (tmpCount > 0 && resultsPerPage > 0)
            {
                totalPages = tmpCount / resultsPerPage;

                // if there is a remainder then there is another page
                if (tmpCount % resultsPerPage != 0)
                {
                    totalPages++;
                }
            }

            return totalPages;
        }
    }
}
