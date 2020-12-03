using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class PartialController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly ISliderService _sliderService;
        private readonly IBulletinService _bulletinService;

        public PartialController(ICategoryService categoryService, ISliderService sliderService, IBulletinService bulletinService)
        {
            _categoryService = categoryService;
            _sliderService = sliderService;
            _bulletinService = bulletinService;
        }

        [HttpPost]
        [Route("bulletin/add")]
        public JsonResult Add(string email)
        {
            var isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                Response.StatusCode = 400;
                return Json("E-Mail formatı hatalı. Lütfen tekrar deneyiniz.", JsonRequestBehavior.AllowGet);
            }

            _bulletinService.Add(email);
            return Json("Bültenimize kaydoldunuz. Gelişmelerden sizleri haberdar edeceğiz. Teşekkürler...", JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult GetMenus()
        {
            var categories = _categoryService.ActiveCategoryList("tr");
            var treeView = Utility.CreateTree(categories).ToList();
            return PartialView("../Partial/Menu", treeView);
        }

        [ChildActionOnly]
        public ActionResult GetMobileMenus()
        {
            var categories = _categoryService.ActiveCategoryList("tr");
            var treeView = Utility.CreateTree(categories).ToList();
            return PartialView("../Partial/MobileMenu", treeView);
        }

        [ChildActionOnly]
        public ActionResult GetSliders()
        {
            var sliders = _sliderService.List();
            return PartialView("../Partial/HomeSlider", sliders);
        }
    }
}