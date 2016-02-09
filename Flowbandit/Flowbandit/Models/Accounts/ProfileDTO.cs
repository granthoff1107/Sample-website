using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models.Accounts
{
    public class ProfileDTO : BaseModel<IUserRepository>
    {
        public string THANKS_FOR_REGISTERING_MESSAGE = "Thanks for registering.  Login and Tell us more about yourself";

        public bool IsInitialLogin = false;

        public ProfileDTO(IUserRepository userRepostory, int id, bool isInitialLogin)
            : base(userRepostory)
        {
            IsInitialLogin = isInitialLogin;
        }
    }
}