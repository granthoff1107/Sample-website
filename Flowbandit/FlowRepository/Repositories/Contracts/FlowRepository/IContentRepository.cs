using FlowRepository.ExtendedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IContentRepository : IFlowRepository
    {
        List<T> GetMostRecentVisibleContent<T>(IQueryable<T> baseQuery, int pageNumber, int resultsPerPage, int currentUser = 0, int? filterUserId = null, bool shouldStripTags = true)
            where T : class, IHasContent;

        T GetVisibleContentByIdWithCommentsTagsUsers<T>(int id, int currentUser = 0)  
            where T : class, IHasContent;

        List<T> SearchContent<T>(string[] searchTerms, int pageNumber, int resultsPerPage, int currentUser = 0)
            where T : class, IHasContent;

        void AddComment(ContentComment comment, int? currentUser = null);
    }
}
