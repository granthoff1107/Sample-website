using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IFriendRepository : IFlowRepository
    {
        List<Friend> GetAllFriends(int userId);
    }
}
