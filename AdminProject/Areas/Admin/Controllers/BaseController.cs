using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private readonly RuntimeSettings _setting;

        public BaseController(RuntimeSettings setting)
        {
            _setting = setting;
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            SetLanguageTag();

            if (Utility.SessionCheck() != null) return;


            requestContext.HttpContext.Response.Clear();
            requestContext.HttpContext.Response.Redirect(Url.Action("Index", "Login"));
            requestContext.HttpContext.Response.End();
        }

        public void SetLanguageTag()
        {
            TempData["LanguageTag"] = _setting.Language.ToUpper();
        }

        public void SetPageHeader(string pageSub, string pageSub1)
        {
            TempData["PageSub"] = pageSub;
            TempData["PageSub1"] = pageSub1;
        }

        public void Added()
        {
            TempData["Success"] = "Added successfully.";
        }

        public void Updated()
        {
            TempData["Success"] = "Updated successfully. <strong>Do not forget to update content in other languages.</strong>";
        }

        public void Deleted()
        {
            TempData["Warning"] = "Deleted successfully. <strong>Do not forget to delete the content in other languages.</strong>";
        }

        public void Warning()
        {
            TempData["Warning"] = "No related records.";
        }
    }
}