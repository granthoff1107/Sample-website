using FlowRepository;
using FlowService.DTOs.Friends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.Services.Contracts
{
    public interface IFriendService
    {
        FriendDTO[] GetAllFriends(int userId);
    }
}
