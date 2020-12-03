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
using Castle.Core.Internal;

namespace AdminProject.Controllers
{
    public class GalleryController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public GalleryController(RuntimeSettings setting, Func<AdminDbContext> dbFactory)
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

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                        extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/Gallery/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(picture, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError",
                                string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", fileName,
                                    ex.Message));
                        }

                        galleryDetail.PictureUrl = pictureName + extension;
                    }
                    else
                        ModelState.AddModelError("Extension",
                            string.Format("Only png, bmp, jpg, jpeg upload. Incorrect file name: {0}", fileName));
                }
                else
                    ModelState.AddModelError("Extension",
                        string.Format("File extension not found. Incorrect file name: {0}", fileName));

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

            ViewBag.StatusList = DropdownTypes.GetStatus(gallery.Status);

            var galleryDetail = db.GalleryDetails.Where(a => a.GalleryId == id).ToList();
            ViewBag.GalleryDetail = galleryDetail;

            return View(gallery);
        }

        [HttpPost]
        public ActionResult Edit(
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
            SetPageHeader("Gallery", "Edit Gallery");
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (!ModelState.IsValid)
                return View();

            var db = _dbFactory();

            var gallery = db.Galleries.FirstOrDefault(a => a.Id == Id);

            if (gallery == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            gallery.CategoryId = 0;
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

                    if (extension != null)
                    {
                        if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" ||
                            extension == ".ico")
                        {
                            var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                            var path = Path.Combine(Server.MapPath("~/Content/Gallery/"), pictureName + extension);
                            try
                            {
                                Utility.FileUpload(picture, path);
                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("PictureUploadError",
                                    string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", fileName,
                                        ex.Message));
                            }

                            galleryDetail.PictureUrl = pictureName + extension;
                        }
                        else
                            ModelState.AddModelError("Extension",
                                string.Format("Only png, bmp, jpg, jpeg upload. Incorrect file name: {0}", fileName));
                    }
                    else
                        ModelState.AddModelError("Extension",
                            string.Format("File extension not found. Incorrect file name: {0}", fileName));

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
    }
}