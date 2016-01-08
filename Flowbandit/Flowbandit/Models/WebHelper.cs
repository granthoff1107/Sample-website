using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Flowbandit.Models
{
    public static class WebHelper
    {
        static string Pattern = "<(?:[^>=]|='[^']*'|=\"[^\"]*\"|=[^'\"][^\\s>]*)*>";

        static public string StripHtml(string Value)
        {
            return Regex.Replace(Value, Pattern, string.Empty);
        }
    }
}