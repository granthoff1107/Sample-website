using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;
using FlowRepository;
using System.Data.Entity;

namespace Flowbandit.Controllers
{
    public class VideosController : BaseController<IVideoRepository>
    {
        //
        // GET: /Video/

        public VideosController()
        {
            var tmpRepo = new VideoRepository();
            InitializerRepository(tmpRepo);
        }

        //protected IVideoRepository Repo
        //{
        //    get
        //    {
        //        return GetRepoAs<IVideoRepository>();
        //    }
        //}

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

        public ActionResult Comment(VideoComment NewComment)
        {
            if (ModelState.IsValid)
            {
                if (!GlobalInfo.IsAnon)
                {
                    NewComment.FK_UserID = GlobalInfo.User.UserID;
                }

                NewComment.Created = DateTime.Now;
                _repository.Add<VideoComment>(NewComment);
                _repository.SaveChanges();
                return RedirectToAction("Video", new { ID = NewComment.FK_VideoID });
            }
            return RedirectToAction("Index", new { ID = 0 });
        }


        public ActionResult TagsAutocomplete(string term)
        {
            try
            {
                var urlhelper = new UrlHelper(this.ControllerContext.RequestContext);

                var results = new List<AutoCompleteResult>();
                results = _repository.TagsStartingWith(term).Select(t => new AutoCompleteResult { label = t.Name, value = t.ID.ToString() }).ToList();

                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult NewVideo()
        {
            return View();
        }

        public ActionResult EditVideo(int ID)
        {
            var tmpViewModel = new VideoVM(_repository, ID);
            return View(tmpViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public JsonResult NewVideo(Video newVideo, List<int> tagIds = null)
        {
            if (ModelState.IsValid && !GlobalInfo.IsAnon)
            {
                newVideo.FK_UserID = GlobalInfo.User.UserID;
                
                if(newVideo.ID == default(int))
                {
                    _repository.Add<Video>(newVideo);
                }
                else
                {
                    _repository.Context.Entry(newVideo).State = EntityState.Modified;
                }

                _repository.SaveChanges();
                return Json( new  { redirectUrl = Url.Action("Video", new { ID = newVideo.ID }) });
            }

            return Json(new { redirectUrl = Url.Action("Index") });
        }

    }
}
