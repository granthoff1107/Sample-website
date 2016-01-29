using Flowbandit.Models;
using FlowRepository;
using FlowRepository.Data.Contracts;
using FlowRepository.Data.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
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
        public AccountsController(IUserRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        //Make this http post only, login data should not go through Query Parameters
        [HttpPost]
        public ActionResult Login(LoginVM LoginData)
        {
            _logRepository.AddInfo(string.Concat("attemped login for user", LoginData.Username, "from IP:", Request.UserHostAddress), "Login");
            if (ModelState.IsValid)
            {
                //TODO: refactor logic into Accounts Repository
                User user = _repository.GetUserByUsername(LoginData.Username);

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

            //TODO: This should always be set from posts however, for testing purposes we'll redirect to home page
            var redirectUrl = (null != HttpContext.Request.UrlReferrer ? HttpContext.Request.UrlReferrer.ToString() : "~");

            return Redirect(redirectUrl);
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
