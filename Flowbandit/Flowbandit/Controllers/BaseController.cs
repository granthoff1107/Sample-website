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
    public class BaseController<TRepository> : BaseController
        where TRepository : IRepository
    {
        protected TRepository _repository;

        protected void InitializeRepository(TRepository DataRepo)
        {
            _repository = DataRepo;
        }
    }

    public class BaseController : Controller
    {
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

        protected string SavePostedFile(HttpPostedFileBase inputFile, string baseFolder)
        {
            string RelativePath = string.Empty;
            string FullPath = string.Empty;

            if (inputFile != null && inputFile.ContentLength > 0)
            {
                FullPath = GetFullPathWithTimestamp(inputFile, baseFolder);
                inputFile.SaveAs(FullPath);
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

        private static string GetFullPathWithTimestamp(HttpPostedFileBase inputFile, string baseFolder)
        {
            string NewFileName = Path.GetFileNameWithoutExtension(inputFile.FileName) + "_" + DateTimeToUnixTimestamp(DateTime.Now);
            string Extension = Path.GetExtension(inputFile.FileName);
            return Path.Combine(GlobalInfo.DownloadsBaseURL, baseFolder, String.Concat(NewFileName, Extension));
        }

        protected static string ToRelativePath(string fullPath)
        {
            return Path.GetFullPath(fullPath).Replace(GlobalInfo.RootDir, string.Empty);
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Math.Floor((double)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
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
