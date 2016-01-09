using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;

namespace Flowbandit.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            var tmpViewModel = new DashboardVM();
            return View(tmpViewModel);
        }

    }
}
