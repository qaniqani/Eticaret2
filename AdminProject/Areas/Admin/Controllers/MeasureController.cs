using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class MeasureController : BaseController
    {
        private readonly IMeasureService _measureService;

        public MeasureController(IMeasureService measureService, RuntimeSettings setting) : base(setting)
        {
            _measureService = measureService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Measure", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(Measure request)
        {
            SetPageHeader("Measure", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View(request);

            var measure = new Measure
            {
                Name = request.Name,
                Status = request.Status
            };

            _measureService.Add(measure);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Measure", "Edit");

            var measure = _measureService.GetMeasure(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(measure.Status);

            return View(measure);
        }

        [HttpPost]
        public ActionResult Edit(Measure request)
        {
            SetPageHeader("Measure", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View(request);

            var measure = _measureService.GetMeasure(request.Id);
            measure.Name = request.Name;
            measure.Status = request.Status;
            _measureService.Edit(request.Id, measure);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Measure", "List");

            var measures = _measureService.AllMeasureList();

            return View(measures);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _measureService.Delete(id);
            Deleted();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult MeasureDetail(int id)
        {
            SetPageHeader("Measure Detail", "Add/ Edit/ List");

            var measure = _measureService.GetMeasure(id);
            var measureDetails = _measureService.AllMeasureDetailList(id);

            ViewBag.Measure = measure;
            ViewBag.MeasureDetails = measureDetails;
            ViewBag.StatusList = DropdownTypes.GetStatus(measure.Status);

            return View();
        }

        [HttpPost]
        public ActionResult MeasureDetail(int id, string Size, StatusTypes Status)
        {
            SetPageHeader("Measure Detail", "Add/ Edit/ List");

            var measure = _measureService.GetMeasure(id);
            var measureDetails = _measureService.AllMeasureDetailList(id);

            ViewBag.Measure = measure;
            ViewBag.MeasureDetails = measureDetails;
            ViewBag.StatusList = DropdownTypes.GetStatus(measure.Status);

            if (string.IsNullOrEmpty(Size))
                ModelState.AddModelError("Size", "Size is required.");

            if (!ModelState.IsValid)
                return View();

            var size = new MeasureDetail
            {
                MeasureId = id,
                Size = Size,
                Status = Status
            };

            _measureService.AddMeasureDetail(size);

            var measureDetails1 = _measureService.AllMeasureDetailList(id);
            ViewBag.MeasureDetails = measureDetails1;

            return RedirectToAction("MeasureDetail", new { id });
        }

        [HttpGet]
        public ActionResult EditMeasureDetail(int id, string command, int detailId)
        {
            if (command == "delete")
            {
                _measureService.DeleteMeasureDetail(detailId);
                return RedirectToAction("MeasureDetail", new {id});
            }

            SetPageHeader("Measure Detail", "Add/ Edit/ List");

            var measure = _measureService.GetMeasure(id);
            var measureDetails = _measureService.AllMeasureDetailList(id);

            var measureDetail = _measureService.GetMeasureDetail(detailId);

            ViewBag.Measure = measure;
            ViewBag.MeasureDetails = measureDetails;
            ViewBag.StatusList = DropdownTypes.GetStatus(measure.Status);

            return View(measureDetail);
        }

        [HttpPost]
        public ActionResult EditMeasureDetail(int id, int detailId, string Size, StatusTypes Status)
        {
            SetPageHeader("Measure Detail", "Add/ Edit/ List");

            var measure = _measureService.GetMeasure(id);

            var measureDetails = _measureService.AllMeasureDetailList(id);

            var measureDetail = _measureService.GetMeasureDetail(detailId);

            ViewBag.MeasureDetails = measureDetails;
            ViewBag.Measure = measure;
            ViewBag.StatusList = DropdownTypes.GetStatus(measure.Status);

            if (string.IsNullOrEmpty(Size))
                ModelState.AddModelError("Size", "Size is required.");

            if (!ModelState.IsValid)
                return RedirectToAction("EditMeasureDetail", new {id, command = "edit", detailId});

            measureDetail.Size = Size;
            measureDetail.Status = Status;

            _measureService.EditMeasureDetail(detailId, measureDetail);

            var measureDetails1 = _measureService.AllMeasureDetailList(id);
            ViewBag.MeasureDetails = measureDetails1;

            return RedirectToAction("MeasureDetail", new { id });
        }
    }
}