using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class PropertyController : BaseController
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService, RuntimeSettings setting) : base(setting)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Property", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(Property request)
        {
            SetPageHeader("Property", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(request);
            }

            _propertyService.Add(request);

            Added();

            return View(request);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Property", "Edit");

            var item = _propertyService.GetProperty(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(item.Status);

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(int id, Property request)
        {
            SetPageHeader("Property", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(request);
            }

            _propertyService.Edit(id, request);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Property Item", "List");

            var properties = _propertyService.AllList();
            return View(properties);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _propertyService.DeleteProperty(id);

            Deleted();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult AddItem(int id)
        {
            SetPageHeader("Property Item", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.PropertyId = id;
            return View(new PropertyItem {PropertyId = id, Status = StatusTypes.Active, SequenceNr = 9999});
        }

        [HttpPost]
        public ActionResult AddItem(int id, PropertyItem request)
        {
            SetPageHeader("Property Item", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(request);
            }

            _propertyService.AddPropertyDetail(request);

            Added();

            return View(request);
        }

        [HttpGet]
        public ActionResult EditItem(int id)
        {
            SetPageHeader("Property Item", "Edit");

            var item = _propertyService.GetPropertyDetail(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(item.Status);

            return View(item);
        }

        [HttpPost]
        public ActionResult EditItem(int id, PropertyItem request)
        {
            SetPageHeader("Property Item", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
            {
                ModelState.AddModelError("Name", "Name is required.");
                return View(request);
            }

            _propertyService.EditPropertyDetail(id, request);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult ListItem(int id)
        {
            SetPageHeader("Property Item", "List");

            var properties = _propertyService.GetPropertyItemList(id);
            return View(properties);
        }

        [HttpGet]
        public ActionResult DeleteItem(int id, string propertyId)
        {
            _propertyService.DeletePropertyItem(id);

            Deleted();

            return RedirectToAction("ListItem",new {id = propertyId});
        }
    }
}