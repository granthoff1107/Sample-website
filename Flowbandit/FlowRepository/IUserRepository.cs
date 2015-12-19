using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository
{
    public interface IUserRepository : IRepository
    {
        User GetUserByUsername(string Username);
    }
}
