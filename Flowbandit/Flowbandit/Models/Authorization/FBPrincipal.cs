using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Flowbandit.Models.Authorization
{
    public class FBPrincipal : IPrincipal
    {

        public IIdentity Identity { get; private set; }

        public bool IsInRole(string role)
        {
            return string.Compare(role, PrivilegeLevel, true) == 0;
        }

        public FBPrincipal(string username, int userId, int privilegeLevelId, string privilegeLevel)
        {
            this.Identity = new GenericIdentity(username);

            this.UserID = userId;
            this.PrivilegeLevelID = privilegeLevelId;
            this.Username = username;
            this.PrivilegeLevel = privilegeLevel;
        }

        public int UserID { get; set; }
        public int PrivilegeLevelID { get; set; }
        public string PrivilegeLevel { get; set; }
        public string Username { get; set; }

    }
}
