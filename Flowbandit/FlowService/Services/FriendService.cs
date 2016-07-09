using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowService.Services.Contracts;
using FlowService.Services.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowRepository;
using FlowService.DTOs.Friends;

namespace FlowService.Services
{
    public class FriendService : ServiceBase<IFriendRepository>, IFriendService
    {
        #region Constructors

        public FriendService(IFriendRepository repo) : base(repo)
        {
        }

        #endregion

        #region IFriendService

        public FriendDTO[] GetAllFriends(int userId)
        {
            var friends = this._repository.GetAllFriends(userId);
            return friends.OrderBy(x => x.User.Username).Select(x => new FriendDTO(x, userId)).ToArray();
        }

        #endregion
    }
}
