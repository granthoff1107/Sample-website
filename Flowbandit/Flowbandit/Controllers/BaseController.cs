using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FlowRepository;
using System.IO;
using Flowbandit.Models;
using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.FlowLog;

namespace Flowbandit.Controllers
{
    public class BaseController<TRepository> : BaseController
        where TRepository : IFlowRepository
    {
        protected TRepository _repository;

        public BaseController(TRepository repository)
        {
            _repository = repository;
            _logRepository = new LogRepository();
        }
    }

    public class BaseController : Controller
    {
        //This is depended consider changing this when you start using IOC container
        protected IFlowLogRepository _logRepository;

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

        protected static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return Math.Floor((double)(dateTime - new DateTime(1970, 1, 1).ToLocalTime()).TotalSeconds);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            //TODO: Get stack trace values from config file
            _logRepository.AddError(filterContext.Exception, Request.Url.OriginalString, true);

            if (filterContext.HttpContext.IsCustomErrorEnabled)
            {
                filterContext.ExceptionHandled = true;
                this.View("Error").ExecuteResult(this.ControllerContext);
            }
        }

    }
}
