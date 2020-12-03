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
    public class SliderController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public SliderController(Func<AdminDbContext> dbFactory, RuntimeSettings setting) : base(setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Slider", "Add New Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoTypes.IsNotVideo);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureTypes.Slider);

            return View();
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(
            VideoTypes VideoType, 
            string Name, 
            string Detail1, 
            string Detail2, 
            string Detail3, 
            string VideoUrl, 
            string VideoEmbedCode, 
            PictureTypes PictureType, 
            HttpPostedFileBase Picture1, 
            HttpPostedFileBase Picture2, 
            HttpPostedFileBase Picture3, 
            HttpPostedFileBase Picture4, 
            HttpPostedFileBase Picture5, 
            HttpPostedFileBase Picture6, 
            HttpPostedFileBase Picture7, 
            HttpPostedFileBase Picture8, 
            HttpPostedFileBase Picture9, 
            HttpPostedFileBase Picture10, 
            string SequenceNumber, 
            StatusTypes Status)
        {
            SetPageHeader("Slider", "Add New Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoType);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (VideoType == VideoTypes.IsEmbedCode)
            {
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

                if (Picture9 == null)
                    ModelState.AddModelError("Thumbnail", "Video thumbnail is required.");
            }

            if (VideoType == VideoTypes.IsVideo)
            {
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

                if (Picture10 == null)
                    ModelState.AddModelError("Thumbnail", "Video thumbnail is required.");
            }

            if (VideoType == VideoTypes.IsNotVideo)
                if (Picture1 == null)
                    ModelState.AddModelError("Picture1", "Picture 1 is required.");

            if (!ModelState.IsValid)
                return View();

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            var slider = new Slider
            {
                CreateDate = DateTime.Now,
                Detail1 = Detail1,
                Detail2 = Detail2,
                Detail3 = Detail3,
                IsVideoLink = VideoType,
                LanguageId = _setting.LanguageId,
                Name = Name,
                PictureType = PictureType,
                SequenceNumber = Convert.ToInt32(SequenceNumber),
                Status = Status,
                VideoEmbedCode = VideoEmbedCode,
                VideoUrl = VideoUrl
            };

            #region //Picture 1
            if (Picture1 != null)
            {
                var fileName = Picture1.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture1.ContentType))
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
                    Utility.FileUpload(Picture1, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture1 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture2.ContentType))
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
                    Utility.FileUpload(Picture2, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture2 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture3.ContentType))
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
                    Utility.FileUpload(Picture3, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture3 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture4.ContentType))
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
                    Utility.FileUpload(Picture4, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture4 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture5.ContentType))
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
                    Utility.FileUpload(Picture5, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture5 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture6.ContentType))
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
                    Utility.FileUpload(Picture6, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture6 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture7.ContentType))
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
                    Utility.FileUpload(Picture7, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture7 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture8.ContentType))
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
                    Utility.FileUpload(Picture8, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture8 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture9.ContentType))
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
                    Utility.FileUpload(Picture9, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture9 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture10.ContentType))
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
                    Utility.FileUpload(Picture10, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture10 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View();
                }
            }
            #endregion

            var db = _dbFactory();
            db.Sliders.Add(slider);
            db.SaveChanges();

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Slider", "Edit Slider");

            var db = _dbFactory();

            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            ViewBag.StatusList = DropdownTypes.GetStatus(slider.Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(slider.IsVideoLink);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(slider.PictureType);

            return View(slider);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(
            int id,
            VideoTypes VideoType,
            string Name,
            string Detail1,
            string Detail2,
            string Detail3,
            string VideoUrl,
            string VideoEmbedCode,
            PictureTypes PictureType,
            HttpPostedFileBase Picture1,
            HttpPostedFileBase Picture2,
            HttpPostedFileBase Picture3,
            HttpPostedFileBase Picture4,
            HttpPostedFileBase Picture5,
            HttpPostedFileBase Picture6,
            HttpPostedFileBase Picture7,
            HttpPostedFileBase Picture8,
            HttpPostedFileBase Picture9,
            HttpPostedFileBase Picture10,
            string SequenceNumber,
            StatusTypes Status)
        {
            var db = _dbFactory();
            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
            {
                ModelState.AddModelError("SliderNotFound", "Slider was not found.");
                return RedirectToAction("List");
            }

            SetPageHeader("Slider", "Edit Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoType);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (Name.Length > 200 || Name.Length < 2)
                ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 2, 200));

            if (VideoType == VideoTypes.IsEmbedCode)
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

            if (VideoType == VideoTypes.IsVideo)
            {
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

                if (VideoUrl.Length < 20)
                    ModelState.AddModelError("VideoUrlLength", string.Format("{0} can be min {1} characters.", "Video Url", 20));
            }

            if (VideoType == VideoTypes.IsNotVideo)
                if (Picture1 == null)
                    ModelState.AddModelError("Picture1", "Picture 1 is required.");

            if (!ModelState.IsValid)
                return View(slider);

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            slider.Detail1 = Detail1;
            slider.Detail2 = Detail2;
            slider.Detail3 = Detail3;
            slider.IsVideoLink = VideoType;
            slider.LanguageId = _setting.LanguageId;
            slider.Name = Name;
            slider.PictureType = PictureType;
            slider.SequenceNumber = Convert.ToInt32(SequenceNumber);
            slider.Status = Status;
            slider.VideoEmbedCode = VideoEmbedCode;
            slider.VideoUrl = VideoUrl;

            #region //Picture 1
            if (Picture1 != null)
            {
                var fileName = Picture1.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture1.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture1, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture1 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture2.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture2, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture2 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture3.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture3, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture3 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture4.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture4, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture4 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture5.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture5, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture5 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture6.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture6, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture6 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture7.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture7, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture7 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture8.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture8, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture8 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture9.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture9, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture9 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (string.IsNullOrEmpty(extension))
                    ModelState.AddModelError("Extension", "File extension not found.");

                if (!_setting.PictureMimeType.Contains(Picture10.ContentType))
                    ModelState.AddModelError("MimeType",
                        string.Format("Only {0} mime type upload.", string.Join(", ", _setting.PictureMimeType)));

                if (!_setting.PictureExtensionTypes.Contains(extension))
                    ModelState.AddModelError("Extension", string.Format("Only {0} upload.", string.Join(", ", _setting.PictureExtensionTypes)));

                if (!ModelState.IsValid)
                    return View(slider);

                var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                try
                {
                    Utility.FileUpload(Picture10, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);
                    slider.Picture10 = pictureName + extension;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("PictureUploadError", ex.Message);
                    return View(slider);
                }
            }
            #endregion

            db.SaveChanges();

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            SetPageHeader("Slider", "List Slider");

            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var sliders = db.Sliders.Where(a => a.LanguageId == languageId).ToList();

            return View(sliders);
        }

        public ActionResult SliderOrder()
        {
            SetPageHeader("Slider", "Slider Order");

            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var sliders = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink == VideoTypes.IsNotVideo).OrderBy(a => a.SequenceNumber).ToList();

            return View(sliders);
        }

        [HttpPost]
        public ActionResult SliderOrder(string[] order)
        {
            SetPageHeader("Slider", "Slider Order");

            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new { Id = id, Number = sequenceNumber };
            });

            var languageId = _setting.LanguageId;
            var slider = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink == VideoTypes.IsNotVideo).ToList();

            slider.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();

            Updated();

            return RedirectToAction("SliderOrder");
        }

        public ActionResult VideoOrder()
        {
            SetPageHeader("Slider", "Video Order");

            var db = _dbFactory();

            var languageId = _setting.LanguageId;

            var sliders = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink != VideoTypes.IsNotVideo).OrderBy(a => a.SequenceNumber).ToList();

            return View(sliders);
        }

        [HttpPost]
        public ActionResult VideoOrder(string[] order)
        {
            SetPageHeader("Slider", "Video Order");

            var db = _dbFactory();

            var list = order.Select(a =>
            {
                var id = Convert.ToInt32(a.Split('|')[0]);
                var sequenceNumber = Convert.ToInt32(a.Split('|')[1]);

                return new { Id = id, Number = sequenceNumber };
            });

            var languageId = _setting.LanguageId;
            var slider = db.Sliders.Where(a => a.LanguageId == languageId && a.IsVideoLink != VideoTypes.IsNotVideo).ToList();

            slider.ForEach(g =>
            {
                var item = list.FirstOrDefault(a => a.Id == g.Id);
                if (item != null)
                    g.SequenceNumber = item.Number;
            });
            db.SaveChanges();

            Updated();

            return RedirectToAction("VideoOrder");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var db = _dbFactory();

            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
            {
                Warning();
                return RedirectToAction("List");
            }

            db.Sliders.Remove(slider);
            db.SaveChanges();

            Deleted();

            return RedirectToAction("List");
        }
    }
}