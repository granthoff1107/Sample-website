using Flowbandit.Models;
using FlowRepository;
using FlowRepository.Data.Contracts;
using FlowRepository.Data.Rules;
using FlowRepository.ExendedModels.Contracts;
using FlowRepository.ExendedModels.Models;
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
            InitializeRepository(tmpRepo);
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

                    User user = _repository.GetUserByUsername(LoginData.Username);
                    //var tempuser = Data.MK3Model.Employees2.Where(x => x.Username == username).SingleOrDefault();

                    if (user != null)
                    {
                        IHash hashRule = new HashRule();
                        if (hashRule.VerifyHash(LoginData.Password, user.PasswordHash))
                        {
                            FBPrincipalSerializeModel serial = new FBPrincipalSerializeModel { UserID = user.ID, PrivilegelevelID = user.FK_PrivilegelevelID };

                            var authcookie = LoginHelper.SerializeObjectToCookie(LoginData.StayLoggedin, user.Username, serial);

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

            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

        //public void UpdateUser(string username, string password)
        //{
        //    IHash hashRule = new HashRule();
        //    var hash = hashRule.CreateHash(password);
        //    var user = _repository.GetUserByUsername(username);
        //    user.PasswordHash = hash;

        //    _repository.SaveChanges();
        //}

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

    }


}
