using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminProject.Helpers
{
    public static class Extensions
    {
        public static int ToInt32(this object obj)
        {
            return Convert.ToInt32(obj);
        }

        public static string ToQueryString(this NameValueCollection @this)
        {
            return ToParameterString(@this, HttpUtility.UrlEncode);
        }

        private static string ToParameterString(NameValueCollection @this, Func<string, string> escaper)
        {
            if (@this == null || @this.Count == 0)
                return string.Empty;

            foreach (var item in @this.AllKeys)
                if (@this[item] == null)
                    @this[item] = string.Empty;

            var nameAndValues = @this.ToEnumerable().Select(e => string.Format("{0}={1}", escaper(e.Key), escaper(e.Value)));
            return string.Join("&", nameAndValues);
        }

        private static string EscapePostParameter(string param)
        {
            return param == null ? string.Empty : param.Replace("&", "%26");
        }

        public static IEnumerable<KeyValuePair<string, string>> ToEnumerable(this NameValueCollection @this)
        {
            return @this.AllKeys.SelectMany(@this.GetValues, (k, v) => new KeyValuePair<string, string>(k, v));
        }

        public static MvcHtmlString MyValidationSummary(this HtmlHelper helper, string validationMessage = "")
        {
            var errorString = "";

            if (helper.ViewData.ModelState.IsValid)
                return new MvcHtmlString("");

            errorString += "<div class='body-nest' id='alert'><h3 style='color:rgba(255, 107, 107, 0.9) !important'>Error!</h3>";

            foreach (var key in helper.ViewData.ModelState.Keys)
            {
                foreach (var err in helper.ViewData.ModelState[key].Errors)
                    errorString += string.Format(@"<div class='alert alert-danger'>
                                                    <button data-dismiss='alert' class='close' type='button'>×</button>
                                                    <span class='entypo-cancel-circled'></span>
                                                    <strong>{0}</strong>
                                                </div>", helper.Raw(err.ErrorMessage));
            }

            errorString += "</div>";

            return new MvcHtmlString(errorString);
        }
    }
}