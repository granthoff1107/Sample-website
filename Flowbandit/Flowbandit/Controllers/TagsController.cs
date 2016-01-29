using FlowRepository;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flowbandit.Controllers
{
    public class TagsController : BaseController<ITagRepository>
    {
        public TagsController(ITagRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        public ActionResult TagsAutocomplete(string term)
        {
            try
            {
                var urlhelper = new UrlHelper(this.ControllerContext.RequestContext);

                var Results = new List<AutoCompleteResult>();

                Results = _repository.TagsStartingWith(term).Select(t => new AutoCompleteResult { label = t.Name, value = t.ID.ToString() }).ToList();

                return Json(Results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }
    }
}