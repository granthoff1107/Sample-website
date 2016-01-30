using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;
using FlowRepository.Repositories.Contracts.FlowRepository;

namespace Flowbandit.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IFlowLogRepository logRepository) 
            : base(logRepository)
        {

        }
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            var tmpViewModel = new DashboardVM();
            return View(tmpViewModel);
        }

    }
}
