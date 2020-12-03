using System.Globalization;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class CargoController : BaseController
    {
        private readonly ICargoService _cargoService;
        private readonly IPictureService _pictureService;

        public CargoController(ICargoService cargoService, RuntimeSettings setting, IPictureService pictureService) : base(setting)
        {
            _cargoService = cargoService;
            _pictureService = pictureService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Cargo", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, string Price, bool IsPayDoor, bool DefaultCargo, StatusTypes Status, HttpPostedFileBase Logo)
        {
            SetPageHeader("Cargo", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Price))
                ModelState.AddModelError("Price", "Price is required.");

            decimal price;
            var ci = new CultureInfo("tr-TR");

            if (!decimal.TryParse(Price, NumberStyles.AllowCurrencySymbol, ci, out price))
                ModelState.AddModelError("Price", "Price is format error.");

            if (!ModelState.IsValid)
                return View();

            var cargo = new Cargo
            {
                DefaultCargo = DefaultCargo,
                IsPayDoor = IsPayDoor,
                Name = Name,
                Price = price,
                Status = Status
            };

            if (Logo != null)
            {
                var uploadPath = _cargoService.GetLogoUploadPath();
                var pictureName = _pictureService.SaveDefaultPicture(Logo, uploadPath);
                cargo.Logo = string.Format("{0}{1}", uploadPath, pictureName);
            }

            _cargoService.Add(cargo);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Cargo", "Edit");

            var cargo = _cargoService.GetCargo(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(cargo.Status);

            return View(cargo);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, string Price, bool IsPayDoor, bool DefaultCargo, StatusTypes Status, HttpPostedFileBase Logo)
        {
            SetPageHeader("Cargo", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            var cargo = _cargoService.GetCargo(id);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Price))
                ModelState.AddModelError("Price", "Price is required.");

            decimal price;
            var ci = new CultureInfo("tr-TR");

            if (!decimal.TryParse(Price, NumberStyles.AllowDecimalPoint, ci, out price))
                ModelState.AddModelError("Price", "Price is format error.");

            if (!ModelState.IsValid)
                return View(cargo);

            if (Logo != null)
            {
                var uploadPath = _cargoService.GetLogoUploadPath();
                var pictureName = _pictureService.SaveDefaultPicture(Logo, uploadPath);
                cargo.Logo = string.Format("{0}{1}", uploadPath, pictureName);
            }

            cargo.DefaultCargo = DefaultCargo;
            cargo.IsPayDoor = IsPayDoor;
            cargo.Name = Name;
            cargo.Price = price;
            cargo.Status = Status;
            _cargoService.Edit(id, cargo);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Cargo", "List");

            var cargos = _cargoService.AllCargoList();

            return View(cargos);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _cargoService.Delete(id);
            Deleted();

            return RedirectToAction("List");
        }
    }
}