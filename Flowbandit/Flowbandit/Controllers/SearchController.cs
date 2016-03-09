using Flowbandit.Models.Search;
using FlowRepository.Repositories.Contracts.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers
{
    public class SearchController : BaseController<IVideoRepository>
    {
        #region Constructors
        
        public SearchController(IVideoRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        #endregion

        // GET: Search
        public ActionResult Index(string q, string repo = "")
        {
            var searchModel = new SearchResultsVM(_repository, q, repo);

            return View();
        }

    }
}