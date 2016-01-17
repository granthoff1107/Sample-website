using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowRepository
{
    public class UserRepository : DataRepository<FlowCollectionEntities>, IUserRepository
    {
        public User GetUserByUsername(string Username)
        {
            return All<User>().FirstOrDefault(u => u.Username.ToLower() == Username.ToLower());
        }
    }
}
