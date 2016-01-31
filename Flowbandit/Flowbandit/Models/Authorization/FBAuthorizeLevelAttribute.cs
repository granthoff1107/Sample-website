using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Flowbandit.Models.Authorization
{
    public class FBAuthorizeLevelAttribute : AuthorizeAttribute
    {
        protected int? _minimumLevel;
        protected int? _maximumLevel;

        //public attribute properties are not allowed to be null
        public int MinimumLevel { get { return _minimumLevel ?? int.MinValue; } set { _minimumLevel = value; } }
        public int MaximumLevel { get{ return _maximumLevel ?? int.MaxValue; } set { _maximumLevel = value; } }

        public string RedirectUrl { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var authorized = base.AuthorizeCore(httpContext);
            var user = (HttpContext.Current.User as FBPrincipal);
            if (false == authorized || null == user)
            {
                return false;
            }

            return (_minimumLevel == null || user.PrivilegeLevelID >= _minimumLevel)
                && (_maximumLevel == null || user.PrivilegeLevelID <= _maximumLevel);
        }

        //TODO implement redirect
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}