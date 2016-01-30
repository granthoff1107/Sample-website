using FlowRepository.Repositories.Contracts.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IFlowLogRepository : IRepository<FlowCollectionLogEntities>
    {
        void AddError(Exception exception, string Url = null,  bool storeStackTrace = true);
        void AddInfo(string message, string ipAddress, string urlRoute, string infoTypeName);
    }
}

