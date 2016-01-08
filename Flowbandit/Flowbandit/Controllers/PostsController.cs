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
    public class PostsController : BaseController
    {
        //
        // GET: /Post/
        public PostsController()
        {
            var tmpRepo = new PostRepository();
            InitializerRepository(tmpRepo);
        }

        protected IPostRepository Repo
        {
            get
            {
                return GetRepoAs<IPostRepository>();
            }
        }

        public ActionResult GetPosts(int pageNumber)
        {
            var tmpViewModel = new AllPostsVM(Repo, pageNumber);
            var res = RenderRazorViewToString("_PostsContent", tmpViewModel);
            return Json(new { htmldata = res }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int PageNumber = 0)
        {
            var tmpViewModel = new AllPostsVM(Repo, PageNumber);
            return View(tmpViewModel);
        }

        public ActionResult Post(int ID)
        {
            var tmpViewModel = new PostVM(Repo, ID);
            return View(tmpViewModel);
        }

        [HttpGet]
        public ActionResult NewPost()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditPost(int ID)
        {
            var tmpViewModel = new PostVM(Repo, ID);
            return View(tmpViewModel);
        }
        
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult NewPost(Post NewPost, HttpPostedFileBase CoverPhoto = null, List<int> TagIDs = null)
        {
            if(ModelState.IsValid && !GlobalInfo.IsAnon)
            {
                if (CoverPhoto != null)
                {
                    var tmpRelative = SavePostedFile(CoverPhoto, "UserResources");

                    if (!string.IsNullOrEmpty(tmpRelative))
                    {
                        NewPost.CoverPhotoUrl = tmpRelative;
                    }
                }

                NewPost.FK_UserID = GlobalInfo.User.UserID;
                if (NewPost.ID == default(int))
                {
                    Repo.Add<Post>(NewPost);
                }
                else
                {
                    Repo.Context.Entry(NewPost).State = EntityState.Modified;
                }
                
                Repo.SaveChanges();
                return RedirectToAction("Post", new { ID = NewPost.ID });
            }
            return RedirectToAction("Index", new { ID = 0 });
        }

        public ActionResult Comment(PostComment NewComment)
        {
            if (ModelState.IsValid )
            {
                if (!GlobalInfo.IsAnon)
                {
                    NewComment.FK_UserID = GlobalInfo.User.UserID;
                }

                NewComment.Created = DateTime.Now;
                Repo.Add<PostComment>(NewComment);
                Repo.SaveChanges();

                return Json(new { redirectUrl = Url.Action("Post", new { ID = NewComment.FK_PostID }) });
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
    }
}
