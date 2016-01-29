﻿using System;
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

namespace Flowbandit.Controllers
{
    public class VideosController : BaseController<IVideoRepository>
    {
        //
        // GET: /Video/
        public VideosController(IVideoRepository repository) : base(repository)
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

        public ActionResult Comment(VideoComment NewComment)
        {
            if (ModelState.IsValid)
            {
                CommentRule.Comment(_repository, NewComment);
                return RedirectToAction("Video", new { ID = NewComment.FK_VideoID });
            }
            return RedirectToAction("Index", new { ID = 0 });
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

                    //TODO Refactor this into a generic method, for this and posts
                    var tagsToRemove = _repository.Context.TagsToVideos.Where(tv => tv.FK_VideoID == newVideo.ID);
                    _repository.Context.TagsToVideos.RemoveRange(tagsToRemove);

                    foreach(var tagId in tagIds)
                    {
                        newVideo.TagsToVideos.Add(new TagsToVideo { FK_VideoID = newVideo.ID, FK_TagID = tagId });
                    }
                }

                _repository.SaveChanges();
                return Json( new  { redirectUrl = Url.Action("Video", new { ID = newVideo.ID }) });
            }

            return Json(new { redirectUrl = Url.Action("Index") });
        }

    }
}
