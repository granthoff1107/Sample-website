using Flowbandit.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Flowbandit.Models
{
    public class LoginHelper
    {
        static public int UserID
        {
            get
            {
                return GlobalInfo.User.UserID;
            }
        }

        static public bool isAnon
        {
            get
            {
                return GlobalInfo.ANONID == UserID;
            }
        }

        static public string Username
        {
            get
            {
                return GlobalInfo.User.Username;
            }
        }


        public static void SetUserFromCookie(HttpCookie authCookie)
        {

            if (authCookie != null)
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var serial = JsonConvert.DeserializeObject<FBPrincipalSerializeModel>(authTicket.UserData);
                HttpContext.Current.User = new FBPrincipal(authTicket.Name, serial.UserID, serial.PrivilegelevelID);
            }
        }

        public static void SetAnonUser()
        {
            HttpContext.Current.User = new FBPrincipal("ANON", GlobalInfo.ANONID, (int)GlobalInfo.PriviledgeLevel.Anon);
        }


        public static HttpCookie SerializeObjectToCookie(bool Persists, string UsersName, object serial)
        {
            string userData = JsonConvert.SerializeObject(serial);


            var authTicket = new FormsAuthenticationTicket(1,
                                                            UsersName,
                                                            DateTime.Now,
                                                            DateTime.Now.AddMonths(3),
                                                            Persists,
                                                            userData,
                                                            FormsAuthentication.FormsCookiePath);

            string encTicket = FormsAuthentication.Encrypt(authTicket);

            var authcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            return authcookie;
        }

    }
}