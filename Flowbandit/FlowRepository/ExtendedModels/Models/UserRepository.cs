using FlowRepository.ExendedModels.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.ExendedModels.Models
{
    public class UserRepository : DataRepository, IUserRepository
    {
        public User GetUserByUsername(string Username)
        {
            return All<User>().FirstOrDefault(u => u.Username.ToLower() == Username.ToLower());
        }
    }
}
