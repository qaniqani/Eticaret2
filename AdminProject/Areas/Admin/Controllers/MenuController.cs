using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class MenuController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly ICategoryService _categoryService;

        public MenuController(RuntimeSettings setting, Func<AdminDbContext> dbFactory, ICategoryService categoryService) : base(setting)
        {
            _setting = setting;
            _dbFactory = dbFactory;
            _categoryService = categoryService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Menu", "Add New Menu");

            GetCategories();
            ViewBag.StatusList = _categoryService.GetStatusType(StatusTypes.Active);
            ViewBag.CategoryTypeList = _categoryService.GetCategoryType(CategoryTypes.Product);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status, CategoryTypes CategoryType)
        {
            SetPageHeader("Menu", "Add New Menu");

            ViewBag.StatusList = _categoryService.GetStatusType(Status);
            ViewBag.CategoryTypeList = _categoryService.GetCategoryType(CategoryType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View();
            }

            var category = new Category
            {
                CategoryType = CategoryType,
                CreateDate = DateTime.Now,
                Description = string.IsNullOrEmpty(Description) ? Name : Description,
                Hit = 0,
                Keyword = string.IsNullOrEmpty(Keyword) ? Name : Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                ModifiedDate = new DateTime(1970, 1, 1),
                Name = Name,
                ParentId = 0,
                SequenceNumber = 9999,
                Status = Status,
                Title = string.IsNullOrEmpty(Title) ? Name : Title,
                Url = Utility.UrlSeo(Name),
                CreateUser = Utility.SessionCheck().Id
            };

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View();

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    category.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }

            _categoryService.AddCategory(category);

            Added();

            GetCategories();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Menu", "Edit Menu");

            GetCategories();

            var cat = _categoryService.GetCategory(id);
            if (cat == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = _categoryService.GetStatusType(cat.Status);
            ViewBag.CategoryTypeList = _categoryService.GetCategoryType(cat.CategoryType);

            return View(cat);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status, CategoryTypes CategoryType)
        {
            var cat = _categoryService.GetCategory(id);

            if (cat == null)
            {
                ModelState.AddModelError("CategoryNotFound", "Category was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Menu", "Edit Menu");

            GetCategories();
            ViewBag.StatusList = _categoryService.GetStatusType(Status);
            ViewBag.CategoryTypeList = _categoryService.GetCategoryType(CategoryType);


            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (!ModelState.IsValid)
                return View(cat);

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(cat);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    cat.Picture = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(cat);
                }
            }

            cat.CategoryType = CategoryType;
            cat.Description = Description;
            cat.Keyword = Keyword;
            cat.ModifiedDate = DateTime.Now;
            cat.Name = Name;
            cat.Status = Status;
            cat.Title = Title;
            cat.Url = Url;
            cat.Status = Status;
            cat.LanguageId = _setting.LanguageId;
            cat.LanguageTag = _setting.Language;
            cat.ModifiedUser = Utility.SessionCheck().Id;

            var db = _dbFactory();
            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _categoryService.DeleteCategory(id);

            Deleted();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult OrderMenu(List<SortedMenu> order)
        {
            if (order == null || !order.Any()) return Json(order, JsonRequestBehavior.AllowGet);

            _categoryService.OrderCategory(order);

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            SetPageHeader("Menu", "All Menus");

            var categories = _categoryService.ListCategory();

            return View(categories);
        }

        public ActionResult Order()
        {
            SetPageHeader("Menu", "Menu Order");

            GetCategories();

            return View();
        }

        public void GetCategories()
        {
            ViewBag.Menu = _categoryService.GetCategories(CategoryTypes.All, StatusTypes.Active);
        }
    }
}