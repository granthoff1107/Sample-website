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
using FlowService.Services;
using FlowRepository.Models.Pagination;
using FlowService.DTOs.Videos;

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
            var videosDTO = this.GetVideosDTO();
            return View(videosDTO);
        }

        public ActionResult GetVideos(int pageNumber)
        {
            var videosDTO = this.GetVideosDTO();
            var res = RenderRazorViewToString("_VideosContent", videosDTO);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Video(int id)
        {
            var videoDTO = this.GetVideoDTO(id);
            return View(videoDTO);
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
        public ActionResult EditVideo(int id)
        {
            var videoDTO = this.GetVideoDTO(id);
            return View(videoDTO);
        }

        [HttpPost]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL)]
        [ValidateInput(false)]
        public JsonResult NewVideo(Video newVideo, List<int> tagIds = null)
        {
            if (ModelState.IsValid)
            {
                tagIds = tagIds ?? new List<int>();
                newVideo.Content.UserId = GlobalInfo.UserId.Value;

                var service = new ContentService<IVideoRepository, Video>(_repository);
                service.ProcessContent(newVideo.Content);

                if (newVideo.Id == default(int))
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

        protected VideosDTO GetVideosDTO()
        {
            var contentService = new ContentService<IVideoRepository, Video>(_repository);
            var pageTrackingInfo = new PageTrackingInfo(GlobalInfo.VIDEOSPERPAGE, 0);
            var videosDTO = contentService.GetVideos(pageTrackingInfo, GlobalInfo.UserId ?? 0);
            return videosDTO;
        }
        protected VideoDTO GetVideoDTO(int id)
        {
            var contentService = new ContentService<IVideoRepository, Video>(_repository);
            var videoDTO = contentService.GetVideo(id, GlobalInfo.UserId ?? 0);
            return videoDTO;
        }
    }
}
