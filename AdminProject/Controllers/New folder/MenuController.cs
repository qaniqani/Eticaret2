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

namespace AdminProject.Controllers
{
    public class MenuController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public MenuController(RuntimeSettings setting, Func<AdminDbContext> dbFactory)
        {
            _setting = setting;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Menu", "Add New Menu");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryTypes.Master);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword,  string Url, StatusTypes Status, CategoryTypes CategoryType)
        {
            SetPageHeader("Menu", "Add New Menu");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryTypes.Master);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (!ModelState.IsValid)
                return View();

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

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                        extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        category.Picture = pictureName + extension;
                    }
                    else
                    {
                        ModelState.AddModelError("Extension", "Only png, bmp, jpg, jpeg upload.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Extension", "File extension not found.");
                    return View();
                }
            }

            var db = _dbFactory();
            db.Categories.Add(category);
            db.SaveChanges();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Menu", "Edit Menu");

            GetCategories();

            var db = _dbFactory();

            var cat = db.Categories.FirstOrDefault(a => a.Id == id);
            if (cat == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(cat.Status);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(cat.CategoryType);

            return View(cat);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status, CategoryTypes CategoryType)
        {
            SetPageHeader("Menu", "Edit Menu");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.CategoryTypeList = DropdownTypes.GetCategoryType(CategoryType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required");

            if (!ModelState.IsValid)
                return View();

            var db = _dbFactory();

            var cat = db.Categories.FirstOrDefault(a => a.Id == id);

            if (cat == null)
            {
                ModelState.AddModelError("CategoryNotFound", "Category was not found.");
                return View();
            }

            if (Picture != null)
            {
                var fileName = Picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                        extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        cat.Picture = pictureName + extension;
                    }
                    else
                    {
                        ModelState.AddModelError("Extension", "Only png, bmp, jpg, jpeg upload.");
                        return View();
                    }
                }
                else
                {
                    ModelState.AddModelError("Extension", "File extension not found.");
                    return View();
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

            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var cat = db.Categories.FirstOrDefault(a => a.Id == id);
            if (cat == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Categories.Remove(cat);
            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult OrderMenu(List<SortedMenu> order)
        {
            if (order == null || !order.Any()) return Json(order, JsonRequestBehavior.AllowGet);

            var db = _dbFactory();

            order.ForEach(a =>
            {
                var row = db.Categories.FirstOrDefault(d => d.Id == a.ItemId);
                if (row == null)
                    return;

                row.ParentId = a.ParentId;
                db.SaveChanges();
            });


            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public ActionResult List()
        {
            SetPageHeader("Menu", "All Menus");

            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var categories = db.Categories.Where(a => a.LanguageId == languageId).ToList();

            return View(categories);
        }

        public void GetCategories()
        {
            var db = _dbFactory();
            var menus = db.Categories.ToList();
            var treeView = Utility.CreateTree(menus).ToList();
            ViewBag.Menu = treeView;
        }
    }
}