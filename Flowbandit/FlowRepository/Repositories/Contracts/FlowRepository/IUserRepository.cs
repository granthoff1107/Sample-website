using FlowRepository.Models.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Contracts.FlowRepository
{
    public interface IUserRepository : IFlowRepository
    {
        User GetUserByUsername(string username);
        User CreateUser(NewUserDTO newUser, int PriviledgeLevelID);
        bool VerifyUser(string username, Guid guid, string verificationName);
    }
}
