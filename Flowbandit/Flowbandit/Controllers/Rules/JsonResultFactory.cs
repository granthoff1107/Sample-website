using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers.Rules
{
    public static class JsonResultFactory
    {
        public static JsonResult GetJsonRedirectResult(string url)
        {
            return new JsonResult { Data = new { redirectUrl = url } };
        }
    }
}