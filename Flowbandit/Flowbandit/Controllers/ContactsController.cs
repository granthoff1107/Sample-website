using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;
using FlowRepository.Repositories.Contracts.FlowRepository;

namespace Flowbandit.Controllers
{
    public class ContactsController : BaseController
    {
        public ContactsController(IFlowLogRepository logRepository)
            : base(logRepository)
        {

        }
        public ActionResult Index()
        {
            var tmpViewModel = new ContactVM();
            return View(tmpViewModel);
        }
    }
}
