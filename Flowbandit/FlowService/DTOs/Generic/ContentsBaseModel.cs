using FlowRepository.ExtendedModels.Contracts;
using FlowRepository.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Generic
{
    public abstract class ContentsBaseModel<T, TDTO> : BaseModel
        where T : IHasContent
    {
        protected abstract Func<T, TDTO> _createEntity { get; }

        public int PageNumber = 0;
        public int TotalPages = 0;
        public IEnumerable<TDTO> FeaturedContent { get; set; }

        #region Constructors

        public ContentsBaseModel(IEnumerable<T> entities, PaginationInfo pageInfo)
        {
            PageNumber = pageInfo.PageNumber;
            TotalPages = pageInfo.TotalPages;
            FeaturedContent = entities.Select(x => this._createEntity(x)).ToList();
        }

        #endregion
    }
}
