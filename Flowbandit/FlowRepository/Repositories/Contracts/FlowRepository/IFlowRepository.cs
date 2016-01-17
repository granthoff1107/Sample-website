using FlowRepository.Repositories.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IFlowRepository : IRepository<FlowCollectionEntities>
    {
    }
}
