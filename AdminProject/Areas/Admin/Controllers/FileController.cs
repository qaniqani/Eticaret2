using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Models;
using File = AdminProject.Infrastructure.Models.File;

namespace AdminProject.Areas.Admin.Controllers
{
    public class FileController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly RuntimeSettings _setting;

        public FileController(Func<AdminDbContext> dbFactory, RuntimeSettings setting) : base(setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("File", "Add New File/ Picture");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            GetCategories();

            return View();
        }

        [HttpPost]
        public ActionResult Add(int CategoryId, string Name, string Description, HttpPostedFileBase FileUrl, StatusTypes Status)
        {
            SetPageHeader("File", "Add New File/ Picture");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            GetCategories();

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (FileUrl == null)
                ModelState.AddModelError("File", "File is required.");

            if (Name.Length > 40 && Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 40));

            if (!ModelState.IsValid)
                return View();

            var file = new File
            {
                CategoryId = CategoryId,
                Description = Description,
                Hit = 0,
                Name = Name,
                Status = Status,
                UploadDate = DateTime.Now,
                UploadUserId = Utility.SessionCheck().Id
            };

            var fileName = FileUrl.FileName;
            var extension = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extension))
                ModelState.AddModelError("Extension", "File extension not found.");

            if (!_setting.FileExtensionTypes.Contains(extension))
                ModelState.AddModelError("Extension",
                    $"Only upload types: {string.Join(",", _setting.FileExtensionTypes)}");

            if (!_setting.FileMimeTypes.Contains(FileUrl.ContentType))
                ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload.", string.Join(", ", _setting.FileMimeTypes)));

            if (!ModelState.IsValid)
                return View();

            var pictureName = Utility.UrlSeo($"{Name}-{DateTime.Now}");
            var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
            try
            {
                Utility.FileUpload(FileUrl, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                file.FileUrl = pictureName + extension;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("FileUploadError", ex.Message);
                return View();
            }

            var db = _dbFactory();
            db.Files.Add(file);
            db.SaveChanges();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("File", "Edit File/ Picture");

            var db = _dbFactory();
            var file = db.Files.FirstOrDefault(a => a.Id == id);

            if (file != null)
            {
                GetCategories();
                ViewBag.StatusList = DropdownTypes.GetStatus(file.Status);
                return View(file);
            }

            Warning();
            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult Edit(int id, int CategoryId, string Name, string Description, HttpPostedFileBase FileUrl, StatusTypes Status)
        {
            var db = _dbFactory();
            var file = db.Files.FirstOrDefault(a => a.Id == id);
            if (file == null)
            {
                ModelState.AddModelError("FileNotFound", "File was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("File", "Edit File/ Picture");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            GetCategories();

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");
            
            if (Name.Length > 40 || Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 40));

            if (!ModelState.IsValid)
                return View(file);

            file.CategoryId = CategoryId;
            file.Description = Description;
            file.Name = Name;
            file.Status = Status;
            file.UploadDate = DateTime.Now;
            file.UploadUserId = Utility.SessionCheck().Id;

            if (FileUrl != null)
            {
                var fileName = FileUrl.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.FileExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension",
                        string.Format("Only upload types: {0}", string.Join(",", _setting.FileExtensionTypes)));

                if (!_setting.FileMimeTypes.Contains(FileUrl.ContentType))
                    ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload.", string.Join(", ", _setting.FileMimeTypes)));

                if (!ModelState.IsValid)
                    return View(file);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}", Name, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(FileUrl, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    file.FileUrl = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("FileUploadError", ex.Message);
                    return View(file);
                }
            }
            
            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            var db = _dbFactory();

            var files = db.Files.OrderByDescending(a => a.Id).ToList();

            return View(files);
        }

        public void GetCategories()
        {
            var db = _dbFactory();
            var languageTag = _setting.Language;
            var menus = db.Categories.Where(a => a.LanguageTag == languageTag).ToList();
            var treeView = Utility.CreateTree(menus).ToList();
            ViewBag.Menu = treeView;
        }
    }
}