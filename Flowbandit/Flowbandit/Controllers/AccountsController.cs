using Flowbandit.Models;
using FlowRepository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Flowbandit.Controllers
{
    public class AccountsController : BaseController<IUserRepository>
    {
        //
        // GET: /Accounts/

        public AccountsController()
        {
            var tmpRepo = new UserRepository();
            InitializerRepository(tmpRepo);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(LoginVM LoginData)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    // Refactor this, only store Encrypted Password in Database and the seed.
                    // When logging only get the seed from the database,  use the seed with the encryption
                    // only send the encrypted password to be matched by a stored procedure in the database.  
                    //Also Have an additional password to be used as a key with the seed internally to the program
                    // This makes it way harder to break security protocol, and 1 will not lose all other passwords

                    var tempuser = _repository.GetUserByUsername(LoginData.Username);
                    //var tempuser = Data.MK3Model.Employees2.Where(x => x.Username == username).SingleOrDefault();

                    if (tempuser != null)
                    {
                        
                        if (tempuser.Password == LoginData.Password)
                        {
                            
                            FBPrincipalSerializeModel serial = new FBPrincipalSerializeModel { UserID = tempuser.ID, PrivilegelevelID = tempuser.FK_PrivilegelevelID };

                            var authcookie = LoginHelper.SerializeObjectToCookie(LoginData.StayLoggedin, tempuser.Username, serial);

                            //string domain = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                            //authcookie.Domain = domain;
                            Response.Cookies.Add(authcookie);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Logger.Instance.WriteToLog("Error Logging in:" + ex.Message);
                }
            }

            return Redirect("~");
        }

      
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~");
        }

    }


}
