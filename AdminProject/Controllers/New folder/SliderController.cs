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
    public class SliderController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public SliderController(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
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

            if (VideoType == VideoTypes.IsEmbedCode)
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

            if (VideoType == VideoTypes.IsVideo)
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

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

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture1, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture1 = pictureName + extension;
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
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture2, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture2 = pictureName + extension;
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
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture3, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture3 = pictureName + extension;
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
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture4, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture4 = pictureName + extension;
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
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture5, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture5 = pictureName + extension;
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
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture6, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture6 = pictureName + extension;
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
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture7, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture7 = pictureName + extension;
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
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture8, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture8 = pictureName + extension;
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
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture9, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture9 = pictureName + extension;
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
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture10, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture10 = pictureName + extension;
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
            SetPageHeader("Slider", "Edit Slider");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.VideoTypeList = DropdownTypes.GetVideoType(VideoType);
            ViewBag.PictureTypeList = DropdownTypes.GetPictureType(PictureType);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (VideoType == VideoTypes.IsEmbedCode)
                if (string.IsNullOrEmpty(VideoEmbedCode))
                    ModelState.AddModelError("VideoEmbedCode", "Video embed code is required.");

            if (VideoType == VideoTypes.IsVideo)
                if (string.IsNullOrEmpty(VideoUrl))
                    ModelState.AddModelError("VideoUrl", "Video url is required.");

            if (VideoType == VideoTypes.IsNotVideo)
                if (Picture1 == null)
                    ModelState.AddModelError("Picture1", "Picture 1 is required.");

            if (!ModelState.IsValid)
                return View();

            if (string.IsNullOrEmpty(SequenceNumber))
                SequenceNumber = "99";

            var db = _dbFactory();
            var slider = db.Sliders.FirstOrDefault(a => a.Id == id);
            if (slider == null)
            {
                ModelState.AddModelError("SliderNotFound", "Slider was not found.");
                return View();
            }

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

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture1, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture1 = pictureName + extension;
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
            #endregion

            #region //Picture 2
            if (Picture2 != null)
            {
                var fileName = Picture2.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture2, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture2 = pictureName + extension;
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
            #endregion

            #region //Picture 3
            if (Picture3 != null)
            {
                var fileName = Picture3.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture3, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture3 = pictureName + extension;
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
            #endregion

            #region //Picture 4
            if (Picture4 != null)
            {
                var fileName = Picture4.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture4, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture4 = pictureName + extension;
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
            #endregion

            #region //Picture 5
            if (Picture5 != null)
            {
                var fileName = Picture5.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture5, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture5 = pictureName + extension;
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
            #endregion

            #region //Picture 6
            if (Picture6 != null)
            {
                var fileName = Picture6.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture6, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture6 = pictureName + extension;
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
            #endregion

            #region //Picture 7
            if (Picture7 != null)
            {
                var fileName = Picture7.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture7, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture7 = pictureName + extension;
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
            #endregion

            #region //Picture 8
            if (Picture8 != null)
            {
                var fileName = Picture8.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture8, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture8 = pictureName + extension;
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
            #endregion

            #region //Picture 9
            if (Picture9 != null)
            {
                var fileName = Picture9.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture9, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture9 = pictureName + extension;
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
            #endregion

            #region //Picture 10
            if (Picture10 != null)
            {
                var fileName = Picture10.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (extension == ".bmp" || extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".ico")
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(Picture10, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("PictureUploadError", ex.Message);
                            return View();
                        }

                        slider.Picture10 = pictureName + extension;
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