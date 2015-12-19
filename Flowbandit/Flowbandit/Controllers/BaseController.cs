using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowRepository;
using System.IO;
using Flowbandit.Models;

namespace Flowbandit.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected IRepository Data;

        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected T GetRepoAs<T>() where T : IRepository
        {

            return (T)(Data);

        }

        protected string SavePostedFile(HttpPostedFileBase InputFile, string BaseFolder)
        {
            string RelativePath = string.Empty;
            string FullPath = string.Empty;

            if (InputFile != null && InputFile.ContentLength > 0)
            {
                FullPath = GetFullPathWithTS(InputFile, BaseFolder);
                InputFile.SaveAs(FullPath);
            }

            if(FullPath != string.Empty)
            {
                RelativePath = ToRelativePath(FullPath);
            }

            return RelativePath;
        }

        public ActionResult SearchAutocomplete(string term)
        {
            try
            {
                var urlhelper = new UrlHelper(this.ControllerContext.RequestContext);

                var Results = new List<AutoCompleteResult>();
            
                return Json(Results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, ex.Message);
            }
        }

        private static string GetFullPathWithTS(HttpPostedFileBase InputFile, string BaseFolder)
        {
            string NewFileName = Path.GetFileNameWithoutExtension(InputFile.FileName) + "_" + DateTimeToUnixTimestamp(DateTime.Now);
            string Extension = Path.GetExtension(InputFile.FileName);
            return Path.Combine(GlobalInfo.DownloadsBaseURL, BaseFolder, String.Concat(NewFileName, Extension));
        }

        protected static string ToRelativePath(string FullPath)
        {
            return Path.GetFullPath(FullPath).Replace(GlobalInfo.RootDir, string.Empty);
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Math.Floor((double)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }

        protected void InitializerRepository(IRepository DataRepo)
        {
            Data = DataRepo;
        }

        protected override ViewResult View(IView view, object model)
        {
            return base.View(view, model);
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            return base.View(viewName, masterName, model);
        }

    }
}
