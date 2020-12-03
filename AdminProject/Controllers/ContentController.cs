using System.Web.Mvc;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    [RoutePrefix("content")]
    public class ContentController : Controller
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService)
        {
            _contentService = contentService;
        }

        [Route("{url}")]
        [OutputCache(Duration = 3600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index(string url)
        {
            var content = _contentService.GetContent(url);
            if (content == null)
                return Redirect("/");

            return View(content);
        }
    }
}