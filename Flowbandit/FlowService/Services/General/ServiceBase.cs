using FlowRepository.Models.Pagination;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services.General
{
    public class ServiceBase<TRepository> where TRepository : class, IFlowRepository
    {
        protected TRepository _repository;

        public ServiceBase(TRepository repository)
        {
            this._repository = repository;
        }
    }
}
