﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Flowbandit.Models;
using FlowRepository;
using System.Data.Entity;

namespace Flowbandit.Controllers
{
    public class VideosController : BaseController
    {
        //
        // GET: /Video/

        public VideosController()
        {

            var tmpRepo = new VideoRepository();
            InitializerRepository(tmpRepo);

        }


        protected IVideoRepository Repo
        {
            get
            {
                return GetRepoAs<IVideoRepository>();
            }
        }


        public ActionResult Index()
        {
            var tmpViewModel = new VideosVM(Repo);
            return View(tmpViewModel);
        
        }

        public ActionResult GetVideos(int pageNumber)
        {
            var tmpViewModel = new VideosVM(Repo, pageNumber);
            var res = RenderRazorViewToString("_VideosContent", tmpViewModel);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Video(int ID)
        {
            var tmpViewModel = new VideoVM(Repo, ID);
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
                Repo.Add<VideoComment>(NewComment);
                Repo.SaveChanges();
                return RedirectToAction("Video", new { ID = NewComment.FK_VideoID });
            }
            return RedirectToAction("Index", new { ID = 0 });
        }


        public ActionResult TagsAutocomplete(string term)
        {
            try
            {
                var urlhelper = new UrlHelper(this.ControllerContext.RequestContext);

                var Results = new List<AutoCompleteResult>();

                Results = Repo.TagsStartingWith(term).Select(t => new AutoCompleteResult { label = t.Name, value = t.ID.ToString() }).ToList();

                return Json(Results, JsonRequestBehavior.AllowGet);
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
            var tmpViewModel = new VideoVM(Repo, ID);
            return View(tmpViewModel);

        }


        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult NewVideo(Video NewVideo, List<int> TagIDs = null)
        {
            if (ModelState.IsValid && !GlobalInfo.IsAnon)
            {

                NewVideo.FK_UserID = GlobalInfo.User.UserID;
                
                if(NewVideo.ID == default(int))
                { 
                    Repo.Add<Video>(NewVideo);
                }
                else
                {
                    Repo.Context.Entry(NewVideo).State = EntityState.Modified;
                }

                Repo.SaveChanges();
                return Json( new  { redirectUrl = Url.Action("Video", new { ID = NewVideo.ID }) });
            }
            return RedirectToAction("Index", new { ID = 0 });
        }

    }
}
