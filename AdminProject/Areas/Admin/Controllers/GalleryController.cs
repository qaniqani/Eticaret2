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

namespace AdminProject.Areas.Admin.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public GalleryController(RuntimeSettings setting, Func<AdminDbContext> dbFactory) : base(setting)
        {
            _setting = setting;
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Gallery", "Add New Gallery");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(
            string Name, 
            string Definition, 
            string Title, 
            string Description, 
            string Keyword, 
            string Url, 
            string SequenceNumber, 
            string[] FileName, 
            string[] FileDescription, 
            HttpPostedFileBase[] FileUpload, 
            StatusTypes Status)
        {
            SetPageHeader("Gallery", "Add New Gallery");
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (FileUpload == null || !FileUpload.Any())
                ModelState.AddModelError("File", "Not selected image. Pictures should be at least 1.");

            if (Name.Length > 200 || Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (!ModelState.IsValid)
                return View();

            var images = FileUpload.Select((picture, index) =>
            {
                var galleryDetail = new GalleryDetail
                {
                    CreateDate = DateTime.Now,
                    Description = FileDescription[index],
                    Name = FileName[index],
                    SequenceNumber = index,
                    Status = StatusTypes.Active
                };

                var fileName = picture.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(picture.ContentType))
                    ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureMimeType), fileName));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureExtensionTypes), fileName));

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/Gallery/"), pictureName + extension);

                try
                {
                    if (!ModelState.IsValid)
                        return galleryDetail;

                    Utility.FileUpload(picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    galleryDetail.PictureUrl = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", fileName, ex.Message));
                }

                return galleryDetail;
            }).ToList();

            if (!ModelState.IsValid)
                return View();

            var db = _dbFactory();

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            var gallery = new Gallery
            {
                CategoryId = 0,
                CreateDate = DateTime.Now,
                Definition = Definition,
                Description = string.IsNullOrEmpty(Description) ? Name : Description,
                Hit = 0,
                Keyword = string.IsNullOrEmpty(Keyword) ? Name : Keyword,
                LanguageId = _setting.LanguageId,
                LanguageTag = _setting.Language,
                Name = Name,
                SequenceNumber = Convert.ToInt32(SequenceNumber),
                Status = Status,
                Title = string.IsNullOrEmpty(Title) ? Name : Title,
                Url = string.IsNullOrEmpty(Url) ? Utility.UrlSeo(Name) : Utility.UrlSeo(Url)
            };

            db.Galleries.Add(gallery);
            db.SaveChanges();

            var galleryId = gallery.Id;

            images.ForEach(a => a.GalleryId = galleryId);

            db.GalleryDetails.AddRange(images);
            db.SaveChanges();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Gallery", "Edit Gallery");

            var db = _dbFactory();

            var gallery = db.Galleries.FirstOrDefault(a => a.Id == id);
            if (gallery == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            GetCategories();

            ViewBag.StatusList = DropdownTypes.GetStatus(gallery.Status);

            var galleryDetail = db.GalleryDetails.Where(a => a.GalleryId == id).ToList();
            ViewBag.GalleryDetail = galleryDetail;

            return View(gallery);
        }

        [HttpPost]
        public ActionResult Edit(
            int CategoryId,
            int Id,
            string Name,
            string Definition,
            string Title,
            string Description,
            string Keyword,
            string Url,
            string SequenceNumber,
            int[] EditPictureId,
            string[] EditFileName,
            string[] EditFileDescription,
            HttpPostedFileBase[] FileUpload,
            string[] FileName,
            string[] FileDescription,
            StatusTypes Status)
        {
            var db = _dbFactory();

            GetCategories();

            var gallery = db.Galleries.FirstOrDefault(a => a.Id == Id);

            if (gallery == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            SetPageHeader("Gallery", "Edit Gallery");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 3)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            if (!ModelState.IsValid)
                return View(gallery);

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            gallery.CategoryId = CategoryId;
            gallery.Definition = Definition;
            gallery.Description = string.IsNullOrEmpty(Description) ? Name : Description;
            gallery.Keyword = string.IsNullOrEmpty(Keyword) ? Name : Keyword;
            gallery.LanguageId = _setting.LanguageId;
            gallery.LanguageTag = _setting.Language;
            gallery.Name = Name;
            gallery.SequenceNumber = Convert.ToInt32(SequenceNumber);
            gallery.Status = Status;
            gallery.Title = string.IsNullOrEmpty(Title) ? Name : Title;
            gallery.Url = string.IsNullOrEmpty(Url) ? Utility.UrlSeo(Name) : Utility.UrlSeo(Url);
            db.SaveChanges();

            //old images
            var oldImages = db.GalleryDetails.Where(a => a.GalleryId == Id).ToList();

            //delete
            var deletedImages = oldImages.Where(a => !EditPictureId.Contains(a.Id) && a.GalleryId == Id).ToList();
            db.GalleryDetails.RemoveRange(deletedImages);

            //edit
            var existingImages = oldImages.Where(a => EditPictureId.Contains(a.Id)).ToList();
            for (var i = 0; i < EditPictureId.Length; i++)
            {
                var pictureId = EditPictureId[i];
                var image = existingImages.FirstOrDefault(a => a.Id == pictureId);

                if (image.Id != pictureId) continue;

                image.Description = EditFileDescription[i];
                image.Name = EditFileName[i];
                image.SequenceNumber = i;
                existingImages[i] = image;
            }

            db.SaveChanges();

            //Add new images
            #region add images
            if (FileUpload.Any() && FileUpload != null)
            {
                if (FileUpload[0] == null)
                {
                    Updated();

                    return RedirectToAction("List");
                }

                var images = FileUpload.Select((picture, index) =>
                {
                    var galleryDetail = new GalleryDetail
                    {
                        CreateDate = DateTime.Now,
                        Description = FileDescription[index],
                        Name = FileName[index],
                        SequenceNumber = index,
                        //SequenceNumber = 99,
                        Status = StatusTypes.Active
                    };

                    var fileName = picture.FileName;
                    var extension = Path.GetExtension(fileName);

                    if (string.IsNullOrEmpty(extension))
                        ModelState.AddModelError("Extension", "File extension not found.");

                    if (!_setting.PictureMimeType.Contains(picture.ContentType))
                        ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureMimeType), fileName));

                    if (!_setting.PictureExtensionTypes.Contains(extension))
                        ModelState.AddModelError("Extension", string.Format("Only {0} upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureExtensionTypes), fileName));

                    var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                    var path = Path.Combine(Server.MapPath("~/Content/Gallery/"), pictureName + extension);

                    try
                    {
                        if (!ModelState.IsValid)
                            return galleryDetail;

                        Utility.FileUpload(picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                        galleryDetail.PictureUrl = pictureName + extension;
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("PictureUploadError", string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", fileName, ex.Message));
                    }

                    return galleryDetail;
                }).ToList();

                images.ForEach(a => a.GalleryId = Id);
                db.GalleryDetails.AddRange(images);
                db.SaveChanges();
            }
            #endregion

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Gallery", "List Gallery");

            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var galleries = db.Galleries.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).ToList();

            return View(galleries);
        }

        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var gallery = db.Galleries.FirstOrDefault(a => a.Id == id);
            if (gallery == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            var galleryDetails = db.GalleryDetails.Where(a => a.GalleryId == id).ToList();

            db.GalleryDetails.RemoveRange(galleryDetails);
            db.Galleries.Remove(gallery);

            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
        }

        public ActionResult Order()
        {
            SetPageHeader("Gallery", "Order Gallery");

            var db = _dbFactory();
            var languageId = _setting.LanguageId;
            var galleries = db.Galleries.Where(a => a.LanguageId == languageId).OrderBy(a => a.SequenceNumber).ToList();

            return View(galleries);
        }

        [HttpPost]
        public ActionResult Order(string[] order)
        {
            SetPageHeader("Gallery", "Order Gallery");

            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new {Id = id, Number = sequenceNumber};
            });

            var languageId = _setting.LanguageId;
            var galleries = db.Galleries.Where(a => a.LanguageId == languageId).OrderByDescending(a => a.Id).ToList();

            galleries.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();

            Updated();

            return RedirectToAction("Order");
        }

        public void GetCategories()
        {
            var db = _dbFactory();
            var menus = db.Categories.Where(a => a.LanguageTag == _setting.Language).ToList();
            var treeView = Utility.CreateTree(menus).ToList();
            ViewBag.Menu = treeView;
        }
    }
}