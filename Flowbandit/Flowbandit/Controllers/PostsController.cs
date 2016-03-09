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
    public class PostsController : BaseController<IPostRepository>
    {
        public PostsController(IPostRepository repository, IFlowLogRepository logRepository)
            : base(repository, logRepository)
        {
        }

        public ActionResult GetPosts(int pageNumber)
        {
            var tmpViewModel = new AllPostsVM(_repository, pageNumber);
            var res = RenderRazorViewToString("_PostsContent", tmpViewModel);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int PageNumber = 0)
        {
            var tmpViewModel = new AllPostsVM(_repository, PageNumber);
            return View(tmpViewModel);
        }

        public ActionResult Post(int ID)
        {
            var tmpViewModel = new PostVM(_repository, ID);
            return View(tmpViewModel);
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Posts/")]
        public ActionResult NewPost()
        {
            return View();
        }

        [HttpGet]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL, RedirectUrl = "~/Posts/Post/{ID}")]
        public ActionResult EditPost(int ID)
        {
            var tmpViewModel = new PostVM(_repository, ID);
            return View(tmpViewModel);
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
                    var tmpRelative = SavePostedFile(CoverPhoto, "UserResources");

                    if (!string.IsNullOrEmpty(tmpRelative))
                    {
                        newPost.CoverPhotoUrl = tmpRelative;
                    }
                }

                //TODO Move this logic into the Post Repository
                newPost.Content.UserId = GlobalInfo.User.UserID;
                newPost.Content.LastModified = DateTime.Now;

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
    }
}
