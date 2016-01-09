using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;

namespace Flowbandit.Controllers
{
    public class ContactsController : BaseController
    {
        //
        // GET: /Contact/

        public ActionResult Index()
        {
            var tmpViewModel = new ContactVM();
            return View(tmpViewModel);
        }

    }
}
