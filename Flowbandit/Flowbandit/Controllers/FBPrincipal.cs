using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Flowbandit.Controllers
{
    public class FBPrincipal : IPrincipal
    {

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            //if (roles.Any(r => role.Contains(r)))
            //{
            //    return true;
            //}
            return false;
        }

        public FBPrincipal(string Username, int UserID, int PrivilegeLevelID)
        {
            this.Identity = new GenericIdentity(Username);

            this.UserID = UserID;
            this.PrivilegelevelID = PrivilegelevelID;
            this.Username = Username;
        }

        public int UserID { get; set; }
        public int PrivilegelevelID { get; set; }
        public string Username { get; set; }

    }
}
