using FlowRepository.ExtendedModels.Contracts;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public class ContentsBaseModel<TRepository, TEntity> : BaseModel<TRepository>
        where TRepository : class, IFlowRepository, IContentRepository
        where TEntity : class, IHasContent
    {
        public int PageNumber = 0;
        public int TotalPages = 0;
        public IEnumerable<TEntity> sanitizedEntities;


        public ContentsBaseModel(TRepository repository, int pageNumber, int resultsPerPage) 
            : base(repository)
        {
            this.PageNumber = pageNumber;

            var tmpCount = repository.All<TEntity>().Count();
            TotalPages = this.GetTotalPageCountFromItems(tmpCount, resultsPerPage);

            var parentContents = repository.GetMostRecentVisibleContent<TEntity>(repository.All<TEntity>(), pageNumber, resultsPerPage, CurrentUser);
            
            foreach(var parentContent in parentContents)
            {
                parentContent.Content.Entry = parentContent.Content.Entry.Substring(0, Math.Min(parentContent.Content.Entry.Length, GlobalInfo.DISPLAY_TEXT_MAX_LENGTH)) + "...";
            }

            sanitizedEntities = parentContents;
        }
    }
}