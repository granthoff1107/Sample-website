using Flowbandit.Models;
using Flowbandit.Models.Authorization;
using FlowRepository;
using FlowRepository.Data.Contracts;
using FlowRepository.Data.Rules;
using FlowRepository.Models.Const;
using FlowRepository.Models.UserRepository;
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

        protected EmailSender _mailSender;

        public AccountsController(IUserRepository repository, IFlowLogRepository logRepository, EmailSender mailSender)
            : base(repository, logRepository)
        {
            _mailSender = mailSender;
        }

        public ActionResult Index()
        {
            return View();
        }

        //Make this http post only, login data should not go through Query Parameters
        [HttpPost]
        public ActionResult Login(LoginVM LoginData)
        {
            _logRepository.AddInfo(string.Concat("attemped login for user", LoginData.Username), Request.UserHostAddress, Request.Url.OriginalString, "Login");
            if (ModelState.IsValid)
            {
                //TODO: refactor logic into Accounts Repository
                User user = _repository.GetUserByUsername(LoginData.Username);

                if (user != null)
                {
                    IHash hashRule = new HashRule();
                    if (hashRule.VerifyHash(LoginData.Password, user.PasswordHash))
                    {
                        var priviledgeLevel = (null != user.PrivilegeLevel ? user.PrivilegeLevel.Name : "");
                        FBPrincipalSerializeModel serial = new FBPrincipalSerializeModel { UserID = user.ID, PrivilegelevelID = user.FK_PrivilegelevelID, PrivilegeLevel = priviledgeLevel };

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

        //TODO Allow anyone to register once the site is secure
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL)]
        public JsonResult CreateUser(NewUserDTO user)
        {
            if(ModelState.IsValid)
            {
                var newUser = _repository.CreateUser(user, BASIC_LEVEL);
                if(null != newUser)
                { 
                    var verficationUrl = GenerateValidateEmailUrl(newUser.UserVerifications.First().VerifiedGuid, newUser.Username);
                    _mailSender.SendEmail(user.Email, "Thanks for registering", "Click The link to verify your email address: " + verficationUrl, "noreply@flowbandit.com");
                }
            }

            //TODO: Send something more meaning full from here
            return Json(new { Success = (null != user).ToString() });
        }

        protected string GenerateValidateEmailUrl(Guid guid, string username)
        {
            return Url.Action("UserVerification", "Accounts", new { guid = guid.ToString(), username = username}, Request.Url.Scheme);
        }

        public ActionResult UserVerification(Guid guid, string username)
        {
            if (_repository.VerifyUser(username, guid, FlowCollectionConsts.VERIFICATION_TYPE_EMAIL))
            {
                return RedirectToAction("ProfilePage");
            }

            //TODO Redirect Appropiately Or throw 
            return Redirect("~");
        }

        [HttpGet]
        public ActionResult ProfilePage(int id)
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

    }


}
