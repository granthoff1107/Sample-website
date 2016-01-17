using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IUserRepository : IFlowRepository
    {
        User GetUserByUsername(string Username);
    }
}
