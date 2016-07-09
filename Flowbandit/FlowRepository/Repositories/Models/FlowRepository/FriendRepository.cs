using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class FriendRepository : DataRepository<FlowCollectionEntities>, IFriendRepository
    {
        public List<Friend> GetAllFriends(int userId)
        {
            return this._context.Friends.Include(x => x.User)
                                .Where(x => x.UserId == userId || x.FriendUserId == userId)
                                .ToList();
        }
    }
}
