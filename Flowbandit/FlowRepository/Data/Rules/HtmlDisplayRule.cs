using Ganss.XSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FlowRepository.Data.Rules
{
    public static class HtmlDisplayRule
    {
        public static string SanitizeHtml(string html, bool isEncoded = true)
        {
            if(isEncoded)
            { 
                html = HttpUtility.HtmlDecode(html);
            }

            var htmlSanitizer = new HtmlSanitizer();
            return htmlSanitizer.Sanitize(html);
            
        }

        public static string StripTags(string html, bool isEncoded = false)
        {
            if (isEncoded)
            {
                html = HttpUtility.HtmlDecode(html);
            }

            string pattern = "<(?:[^>=]|='[^']*'|=\"[^\"]*\"|=[^'\"][^\\s>]*)*>";
            return Regex.Replace(html, pattern, string.Empty);
        }
    }
}