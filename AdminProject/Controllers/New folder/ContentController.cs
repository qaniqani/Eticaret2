using System;
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
    public class ContentController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public ContentController(RuntimeSettings setting, Func<AdminDbContext> dbFactory)
        {
            _setting = setting;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Content", "Add New Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentTypes.Content);

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(int CategoryId, ContentTypes ContentType, string Name, string Detail, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status)
        {
            SetPageHeader("Content", "Add New Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Subject", "Subject is required.");

            if (CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Category is not selected.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (!ModelState.IsValid)
                return View();

            //escape html tags
            var detailText = Utility.StripHtml(Detail);

            var content = new Content
            {
                CategoryId = CategoryId,
                ContentType = ContentType,
                CreateDate = DateTime.Now,
                CreateUser = Utility.SessionCheck().Id,
                Description =
                    string.IsNullOrEmpty(Description)
                        ? (detailText.Length > 160 ? detailText.Substring(0, 160) : detailText)
                        : Description,
                Detail = Detail,
                Hit = 0,
                Keyword = string.IsNullOrEmpty(Keyword) ? Name.Replace(' ', ',') : Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                ModifiedDate = new DateTime(1970, 1, 1),
                ModifiedUser = 0,
                Name = Name,
                SequenceNumber = 9999,
                Status = Status,
                Title = string.IsNullOrEmpty(Title) ? Name : Title,
                Url = string.IsNullOrEmpty(Url) ? Utility.UrlSeo(Name) : Utility.UrlSeo(Url)
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

                        content.Picture = pictureName + extension;
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
            db.Contents.Add(content);
            db.SaveChanges();

            Added();

            return View(content);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Content", "Edit Content");

            GetCategories();

            var db = _dbFactory();
            var content = db.Contents.FirstOrDefault(a => a.Id == id);
            if (content == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(content.Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(content.ContentType);

            return View(content);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int Id, int CategoryId, ContentTypes ContentType, string Name, string Detail, HttpPostedFileBase Picture, string Title, string Description, string Keyword, string Url, StatusTypes Status)
        {
            SetPageHeader("Content", "Edit Content");

            GetCategories();
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ContentTypeList = DropdownTypes.GetContentType(ContentType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Subject", "Subject is required.");

            if (CategoryId == 0)
                ModelState.AddModelError("CategoryId", "Category is not selected.");

            if (string.IsNullOrEmpty(Detail))
                ModelState.AddModelError("Detail", "Detail is required.");

            if (!ModelState.IsValid)
                return View();
            
            var db = _dbFactory();
            var content = db.Contents.FirstOrDefault(a => a.Id == Id);

            if (content == null)
            {
                ModelState.AddModelError("ContentNotFound", "Content was not found.");
                return View();
            }

            content.CategoryId = CategoryId;
            content.ContentType = ContentType;
            content.Description = Description;
            content.Detail = Detail;
            content.Keyword = Keyword;
            content.LanguageId = _setting.LanguageId;
            content.LanguageTag = _setting.Language;
            content.ModifiedDate = DateTime.Now;
            content.ModifiedUser = Utility.SessionCheck().Id;
            content.Name = Name;
            content.SequenceNumber = 9999;
            content.Status = Status;
            content.Title = Title;
            content.Url = Utility.UrlSeo(Url);

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

                        content.Picture = pictureName + extension;
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

            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Content", "List Content");

            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var contents = db.Contents.Where(a => a.LanguageId == languageId).ToList().Select(a =>
            {
                a.Detail = Utility.StripHtml(a.Detail);
                return a;
            });

            return View(contents);
        }

        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var content = db.Contents.FirstOrDefault(a => a.Id == id);
            if (content == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Contents.Remove(content);
            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
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