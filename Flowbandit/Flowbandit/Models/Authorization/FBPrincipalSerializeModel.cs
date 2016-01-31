using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Flowbandit.Models.Authorization
{
    class FBPrincipalSerializeModel
    {
        public int UserID { get; set; }
        public int PrivilegelevelID { get; set; }
        public string PrivilegeLevel { get; set; }
    }
}
