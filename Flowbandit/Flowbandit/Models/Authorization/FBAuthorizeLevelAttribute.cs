using Flowbandit.Controllers.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


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

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            if (filterContext.ActionDescriptor is ReflectedActionDescriptor)
            {
                var actionDescriptor = (filterContext.ActionDescriptor as ReflectedActionDescriptor);

                if (false == string.IsNullOrWhiteSpace(RedirectUrl))
                {
                    //TODO Support Query String when searching for values
                    var caseInsensitiveRouteMap = filterContext.RouteData.Values
                                                    .ToDictionary(k => k.Key, v => v.Value, 
                                                    StringComparer.OrdinalIgnoreCase);
                    var url = BuildUrl(RedirectUrl, caseInsensitiveRouteMap);
                   
                    if(actionDescriptor.MethodInfo.ReturnType == typeof(JsonResult))
                    {
                        filterContext.Result = JsonResultFactory.GetJsonRedirectResult(url); 
                    }
                    else if (actionDescriptor.MethodInfo.ReturnType == typeof(ActionResult))
                    {
                        filterContext.Result = new RedirectResult(url);
                    }
                }
            }
        }

        protected string BuildUrl(string redirectUrl, Dictionary<string,Object> parameterMap)
        {
            var url = redirectUrl;
            string parameterName = null;

            while(TryGetParameterName(url, out parameterName))
            {
                if(parameterMap.ContainsKey(parameterName))
                {
                    var value = parameterMap[parameterName];
                    url = url.Replace("{" + parameterName + "}", value.ToString());
                }
            }

            return url;
        }

        protected bool TryGetParameterName(string url, out string parameterName)
        {
            parameterName = string.Empty;
            var match = Regex.Match(url, @"\{([^}]+)\}");
            if (match.Success)
            { 
                parameterName = match.Value.Substring(1, match.Value.Length - 2);
            }
            return match.Success;
        }
    }
}