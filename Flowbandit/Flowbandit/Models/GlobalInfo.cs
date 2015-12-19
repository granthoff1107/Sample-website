using Flowbandit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Models
{
    public static class GlobalInfo
    {
        //TODO Move these into their own config file
        public const string WebsiteName = "Flowbandit.com";
        public const string CompanyName = "Flowbandit LLC";
        public const string ContactAddressLine1 = "123 Fake St";
        public const string ContactAddresLine2 = "Everywhere NYC, 10110";
        public const string ContactEmail = "youremail@Flowbandit.com";
        public const string ContactPhone = "212-555-5555";

        public const int RESULTSPERPAGE = 5;
        public const int ANONID = 0;

        public const int VIDEOSPERPAGE = 12;


        private static string _downloadsBaseURL;
        private static string _rootDir;

        public static string DownloadsBaseURL
        {
            get
            {
                if (string.IsNullOrEmpty(_downloadsBaseURL))
                {
                    _downloadsBaseURL = HttpContext.Current.Server.MapPath("~/Content/");
                }

                return _downloadsBaseURL;
            }
        }

        public static string RootDir
        {
            get
            {
                if (string.IsNullOrEmpty(_rootDir))
                {
                    _rootDir = HttpContext.Current.Server.MapPath("~");
                }

                return _rootDir;
            }
        }

        public static FBPrincipal User
        {
            get
            {
                return (HttpContext.Current.User as FBPrincipal);
            }
        }

        public static bool IsAnon
        {
            get
            {
                return (HttpContext.Current.User as FBPrincipal).UserID == ANONID;
            }
        }


        public enum PriviledgeLevel
        {
            Owner = 1,
            Admin = 2,
            Basic = 3,
            Anon = 4
        }
    }
}