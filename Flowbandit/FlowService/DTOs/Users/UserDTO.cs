using FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowService.DTOs.Users
{
    public class UserDTO
    {
        User user;

        public UserDTO(User user)
        {
            this.user = user;  
        }

        public string Username
        {
            get { return user.Username; }
        }

        public string Email
        {
            get { return user.Email; }
        }
    }
}
