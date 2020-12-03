using System.Linq;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Models;
using AdminProject.Services.Interface;
using reCAPTCHA.MVC;

namespace AdminProject.Controllers
{
    public class DefaultController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IEmailService _emailService;

        public DefaultController(IProductService productService, IEmailService emailService, ICategoryService categoryService)
        {
            _productService = productService;
            _emailService = emailService;
            _categoryService = categoryService;
        }

        [Route("")]
        [CookieCheck]
        public ActionResult Index()
        {
            //ViewBag.Category = _categoryService.GetCategoryParentList(0).Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.SequenceNumber).ToList();

            return View();
        }

        [HttpGet]
        [Route("contact")]
        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("contact")]
        [CaptchaValidator]
        public ActionResult Contact(string nameSurname, string companyName, string email, string phone, string message, bool captchaValid)
        {
            if (string.IsNullOrEmpty(nameSurname))
                ModelState.AddModelError("nameSurname", "İsim soyisim zorunludur");

            if (string.IsNullOrEmpty(email))
                ModelState.AddModelError("email", "Email zorunludur");

            if (string.IsNullOrEmpty(message))
                ModelState.AddModelError("message", "Mesaj zorunludur");

            if(!captchaValid)
                ModelState.AddModelError("captcha", "Doğrulama kodu hatalı");

            if (!ModelState.IsValid)
            {
                ErrorMessage(GetErrorMessage(ModelState.Values));
                return View();
            }

            _emailService.SendContactMail(nameSurname, companyName, email, phone, message);

            SuccessMessage("Formunuz başarıyla gönderilmiştir. Teşekkürler.");

            return View();
        }

        [ChildActionOnly]
        public ActionResult FeaturedItems()
        {
            var items = _productService.GetFeaturedItems();

            return PartialView("../Partial/HomeFeaturedItems", items);
        }

        [ChildActionOnly]
        public ActionResult LastAddedItems()
        {
            var items = _productService.GetLastAddedItems();

            return PartialView("../Partial/HomeLastAddedItems", items);
        }

        [ChildActionOnly]
        public ActionResult TopRatedItems()
        {
            var items = _productService.GetTopRatedItems();

            return PartialView("../Partial/HomeTopRatedItems", items);
        }
    }
}