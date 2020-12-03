using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService, RuntimeSettings setting) : base(setting)
        {
            _brandService = brandService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Brand", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(Brand request)
        {
            SetPageHeader("Brand", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View(request);

            var brand = new Brand
            {
                Name = request.Name,
                Status = request.Status
            };

            _brandService.Add(brand);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Brand", "Edit");

            var brand = _brandService.GetBrand(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(brand.Status);

            return View(brand);
        }

        [HttpPost]
        public ActionResult Edit(Brand request)
        {
            SetPageHeader("Brand", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View(request);

            var brand = _brandService.GetBrand(request.Id);
            brand.Name = request.Name;
            brand.Status = request.Status;
            _brandService.Edit(request.Id, brand);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Brand", "List");

            var brands = _brandService.AllBrandList();

            return View(brands);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _brandService.Delete(id);
            Deleted();
            return RedirectToAction("List");
        }
    }
}