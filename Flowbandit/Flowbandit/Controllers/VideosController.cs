using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;
using FlowRepository;
using System.Data.Entity;
using Flowbandit.Controllers.Rules;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowRepository;
using Flowbandit.Models.Authorization;

namespace Flowbandit.Controllers
{
    public class VideosController : BaseController<IVideoRepository>
    {
        //
        // GET: /Video/
        public VideosController(IVideoRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        public ActionResult Index()
        {
            var tmpViewModel = new VideosVM(_repository);
            return View(tmpViewModel);
        }

        public ActionResult GetVideos(int pageNumber)
        {
            var tmpViewModel = new VideosVM(_repository, pageNumber);
            var res = RenderRazorViewToString("_VideosContent", tmpViewModel);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Video(int ID)
        {
            var tmpViewModel = new VideoVM(_repository, ID);
            return View(tmpViewModel);
        }

        public ActionResult Comment(ContentComment newComment)
        {
            if (ModelState.IsValid)
            {
                _repository.AddComment(newComment, GlobalInfo.UserId);
                _repository.SaveChanges();
            }

            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Videos/")]
        public ActionResult NewVideo()
        {
            return View();
        }

        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Videos/Video/{ID}")]
        public ActionResult EditVideo(int ID)
        {
            var tmpViewModel = new VideoVM(_repository, ID);
            return View(tmpViewModel);
        }

        [HttpPost]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL)]
        [ValidateInput(false)]
        public JsonResult NewVideo(Video newVideo, List<int> tagIds = null)
        {
            if (ModelState.IsValid)
            {
                tagIds = tagIds ?? new List<int>();
                newVideo.Content.UserId = GlobalInfo.User.UserID;
                
                if(newVideo.Id == default(int))
                {
                    _repository.Add(newVideo);
                }
                else
                {
                    _repository.EditVideo(newVideo, tagIds);
                }

                _repository.SaveChanges();

                return GetJsonRedirectResult(Url.Action("Video", new { ID = newVideo.Id }));
            }

            return  GetJsonRedirectResult(Url.Action("Index"));
        }

    }
}
