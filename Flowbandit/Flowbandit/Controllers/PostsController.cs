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
using Flowbandit.Models.Authorization;
using FlowService.Services;
using FlowRepository.Models.Pagination;
using FlowService.DTOs.Posts;

namespace Flowbandit.Controllers
{
    public class PostsController : BaseController<IPostRepository>
    {
        public PostsController(IPostRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        public ActionResult GetPosts(int pageNumber)
        {
            var posts = this.GetPostsDTO(pageNumber);
            var res = RenderRazorViewToString("_PostsContent", posts);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int pageNumber = 0)
        {
            var posts = this.GetPostsDTO(pageNumber);
            return View(posts);
        }

        public ActionResult Post(int id)
        {
            var postDTO = this.GetPostDTO(id);
            return View(postDTO);
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Posts/")]
        public ActionResult NewPost()
        {
            return View();
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Posts/Post/{ID}")]
        public ActionResult EditPost(int id)
        {
            var postDTO = this.GetPostDTO(id);
            return View(postDTO);
        }

        //TODO Refactor to use DTO so controllers are not dependent on Database objects
        [HttpPost]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL)]
        [ValidateInput(false)]
        public JsonResult NewPost(Post newPost, HttpPostedFileBase CoverPhoto = null, List<int> tagIds = null)
        {
            if(ModelState.IsValid)
            {
                tagIds = tagIds ?? new List<int>();

                //TODO Move this logic into the Base Repository
                if (CoverPhoto != null)
                {
                    var tmpRelative = this.SavePostedFile(CoverPhoto, "UserResources");

                    if (!string.IsNullOrEmpty(tmpRelative))
                    {
                        newPost.CoverPhotoUrl = tmpRelative;
                    }
                }

                //TODO Move this logic into the Content Service
                newPost.Content.UserId = GlobalInfo.UserId.Value;

                var service = new ContentService<IPostRepository, Post>(_repository);
                service.ProcessContent(newPost.Content);

                if (newPost.Id == default(int))
                {
                    _repository.Add(newPost);
                }
                else
                {
                    _repository.EditPost(newPost, tagIds);
                }

                _repository.SaveChanges();
                return  GetJsonRedirectResult(Url.Action("Post", new { ID = newPost.Id })); 
            }
            return  GetJsonRedirectResult(Url.Action("Index"));
        }

        public ActionResult Comment(ContentComment newComment)
        {
            if (ModelState.IsValid )
            {
                _repository.AddComment(newComment, GlobalInfo.UserId);
                _repository.SaveChanges();
            }
            
            return Redirect(HttpContext.Request.UrlReferrer.ToString());
        }

        protected PostsDTO GetPostsDTO(int pageNumber)
        {
            var contentService = new ContentService<IPostRepository, Post>(_repository);
            var pageTrackingInfo = new PageTrackingInfo(GlobalInfo.RESULTSPERPAGE, pageNumber);
            var posts = contentService.GetPosts(pageTrackingInfo, GlobalInfo.UserId ?? 0);
            return posts;
        }

        protected PostDTO GetPostDTO(int id)
        {
            var contentService = new ContentService<IPostRepository, Post>(_repository);
            var postDTO = contentService.GetPost(id, GlobalInfo.UserId ?? 0);
            return postDTO;
        }
    }
}
