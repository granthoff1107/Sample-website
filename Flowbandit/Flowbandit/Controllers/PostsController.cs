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

        //
        // GET: /Post/
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
        
        [HttpPost]
        [FBAuthorizeLevel(MaximumLevel = ADMIN_LEVEL)]
        [ValidateInput(false)]
        public JsonResult NewPost(Post NewPost, HttpPostedFileBase CoverPhoto = null, List<int> tagIds = null)
        {
            if(ModelState.IsValid)
            {
                //TODO Move this logic into the Base Repository
                if (CoverPhoto != null)
                {
                    var tmpRelative = SavePostedFile(CoverPhoto, "UserResources");

                    if (!string.IsNullOrEmpty(tmpRelative))
                    {
                        NewPost.CoverPhotoUrl = tmpRelative;
                    }
                }

                //TODO Move this logic into the Post Repository
                NewPost.FK_UserID = GlobalInfo.User.UserID;
                NewPost.Last_Modified = DateTime.Now;

                if (NewPost.ID == default(int))
                {
                    _repository.Add(NewPost);
                }
                else
                {
                    _repository.EditPost(NewPost, tagIds);
                }

                _repository.SaveChanges();
                return  GetJsonRedirectResult(Url.Action("Post", new { ID = NewPost.ID })); 
            }
            return  GetJsonRedirectResult(Url.Action("Index"));
        }

        public ActionResult Comment(PostComment NewComment)
        {
            if (ModelState.IsValid )
            {
                CommentRule.Comment(_repository, NewComment);
                return RedirectToAction("Post", new { ID = NewComment.FK_PostID });
            }
            return RedirectToAction("Index", new { ID = 0 });
        }
    }
}
