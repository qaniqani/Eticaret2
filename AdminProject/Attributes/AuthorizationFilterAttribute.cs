using System.Web.Mvc;
using AdminProject.Helpers;

namespace AdminProject.Attributes
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Utility.UserCheck() == null)
            {
                var url = $"/user/login?returnUrl={filterContext.HttpContext.Request.RawUrl}";
                filterContext.Result = new RedirectResult(url);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}