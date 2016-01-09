using Flowbandit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flowbandit.Rules
{
    public static class HtmlDisplayRule
    {
        public static string GetSanitizedText(string text, int maxLength)
        {
            var sanitizedDescription = HttpUtility.HtmlDecode(text);
            sanitizedDescription = WebHelper.StripHtml(sanitizedDescription);

            var subStringLength =  Math.Min(maxLength, sanitizedDescription.Length);
            return sanitizedDescription.Substring(0, subStringLength);
        }
    }
}