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
    public class MultiGalleryController : BaseController
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public MultiGalleryController(RuntimeSettings setting, Func<AdminDbContext> dbFactory) : base(setting)
        {
            _setting = setting;
            _dbFactory = dbFactory;
        }
        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Gallery", "Add New Gallery");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            var db = _dbFactory();
            var languages = db.Languages.Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.Name).ToList();

            languages.ForEach(a => GetCategories(a.UrlTag));

            ViewBag.Languages = languages;

            return View();
        }

        [HttpPost]
        public ActionResult Add(HttpPostedFileBase[] FileUpload)
        {
            var db = _dbFactory();
            var languages = db.Languages.Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.Name).ToList();

            var form = Request.Form;
            SetPageHeader("Gallery", "Add New Gallery");
            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.Languages = languages;

            languages.ForEach(a => GetCategories(a.UrlTag));

            if (!FileUpload.Any() || FileUpload == null)
            {
                ModelState.AddModelError("FileUpload", "File is required.");
                return View();
            }

            const string categoryId = "CategoryId_{0}";
            const string name = "Name_{0}";
            const string definition = "Definition_{0}";
            const string title = "Title_{0}";
            const string description = "Description_{0}";
            const string keyword = "Keyword_{0}";
            const string url = "Url_{0}";
            const string sequenceNumber = "SequenceNumber_{0}";
            const string status = "Status_{0}";
            const string fileName = "FileName_{0}";
            const string fileDescription = "FileDescription_{0}";

            if (languages.Any())
            {
                var images = FileUpload.Select((picture, index) =>
                {
                    var galleryDetail = new GalleryDetail
                    {
                        CreateDate = DateTime.Now,
                        SequenceNumber = index,
                        Status = StatusTypes.Active
                    };

                    var pictureFileName = picture.FileName;
                    var extension = Path.GetExtension(pictureFileName);

                    if (string.IsNullOrEmpty(extension))
                        ModelState.AddModelError("Extension", string.Format("File extension not found. File name: {0}", pictureFileName));

                    if (!_setting.PictureMimeType.Contains(picture.ContentType))
                        ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureMimeType), pictureFileName));

                    if (!_setting.PictureExtensionTypes.Contains(extension))
                        ModelState.AddModelError("Extension", string.Format("Only {0} upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureExtensionTypes), pictureFileName));

                    var pictureName = Utility.UrlSeo(string.Format("{0}-{1}", pictureFileName, DateTime.Now));
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
                        ModelState.AddModelError("PictureUploadError", string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", pictureFileName, ex.Message));
                    }

                    return galleryDetail;
                }).ToList();

                if (!ModelState.IsValid)
                    return View();

                foreach (var language in languages)
                {
                    var categoryIdLanguageControl = string.Format(categoryId, language.UrlTag);
                    var nameLanguageControl = string.Format(name, language.UrlTag);
                    var definitionLanguageControl = string.Format(definition, language.UrlTag);
                    var titleLanguageControl = string.Format(title, language.UrlTag);
                    var descriptionLanguageControl = string.Format(description, language.UrlTag);
                    var keywordLanguageControl = string.Format(keyword, language.UrlTag);
                    var urlLanguageControl = string.Format(url, language.UrlTag);
                    var sequenceNumberLanguageControl = string.Format(sequenceNumber, language.UrlTag);
                    var statusLanguageControl = string.Format(status, language.UrlTag);
                    var fileNameLanguageControl = string.Format(fileName, language.UrlTag);
                    var fileDescriptionLanguageControl = string.Format(fileDescription, language.UrlTag);

                    var categoryIdValue = form[categoryIdLanguageControl];
                    if (categoryIdValue == "0" || string.IsNullOrEmpty(categoryIdValue))
                        continue;

                    var nameValue = form[nameLanguageControl];
                    var definitionValue = form[definitionLanguageControl];
                    var titleValue = form[titleLanguageControl];
                    var descriptionValue = form[descriptionLanguageControl];
                    var keywordValue = form[keywordLanguageControl];
                    var urlValue = form[urlLanguageControl];
                    var sequenceNumberValue = form[sequenceNumberLanguageControl];
                    var statusValue = (StatusTypes)Convert.ToInt32(form[statusLanguageControl]);

                    var fileNameValue = form.GetValues(fileNameLanguageControl);
                    var fileDescriptionValue = form.GetValues(fileDescriptionLanguageControl);
                    
                    if (string.IsNullOrEmpty(nameValue))
                    {
                        ModelState.AddModelError("Name", "Name is required.");
                        break;
                    }

                    if (string.IsNullOrEmpty(sequenceNumberValue))
                        sequenceNumberValue = "99";

                    var gallery = new Gallery
                    {
                        CategoryId = Convert.ToInt32(categoryIdValue),
                        CreateDate = DateTime.Now,
                        Definition = definitionValue,
                        Description = string.IsNullOrEmpty(descriptionValue) ? nameValue : descriptionValue,
                        Hit = 0,
                        Keyword = string.IsNullOrEmpty(keywordValue) ? nameValue : keywordValue,
                        LanguageId = language.Id,
                        LanguageTag = language.UrlTag,
                        Name = nameValue,
                        SequenceNumber = Convert.ToInt32(sequenceNumberValue),
                        Status = statusValue,
                        Title = string.IsNullOrEmpty(titleValue) ? nameValue : titleValue,
                        Url = string.IsNullOrEmpty(urlValue) ? Utility.UrlSeo(nameValue) : Utility.UrlSeo(urlValue)
                    };

                    db.Galleries.Add(gallery);
                    db.SaveChanges();

                    var galleryId = gallery.Id;

                    for (var index = 0; index < images.Count; index++)
                    {
                        images[index].GalleryId = galleryId;
                        images[index].Name = fileNameValue[index];
                        images[index].Description = fileDescriptionValue[index];
                    }

                    db.GalleryDetails.AddRange(images);
                    db.SaveChanges();
                }
            }

            if (!ModelState.IsValid)
                return View();

            Added();

            return View();

            #region //old code
            //if (string.IsNullOrEmpty(Name))
            //    ModelState.AddModelError("Name", "Name is required.");

            //if (FileUpload == null || !FileUpload.Any())
            //    ModelState.AddModelError("File", "Not selected image. Pictures should be at least 1.");

            //if (Name.Length > 200 || Name.Length < 3)
            //    ModelState.AddModelError("NameLength", string.Format("At least {1} {0} can be max {2} characters.", "Name", 3, 200));

            //if (!ModelState.IsValid)
            //    return View();

            //var images = FileUpload.Select((picture, index) =>
            //{
            //    var galleryDetail = new GalleryDetail
            //    {
            //        CreateDate = DateTime.Now,
            //        Description = FileDescription[index],
            //        Name = FileName[index],
            //        SequenceNumber = index,
            //        Status = StatusTypes.Active
            //    };

            //    var fileName = picture.FileName;
            //    var extension = Path.GetExtension(fileName);

            //    if (string.IsNullOrEmpty(extension))
            //        ModelState.AddModelError("Extension", "File extension not found.");

            //    if (!_setting.PictureMimeType.Contains(picture.ContentType))
            //        ModelState.AddModelError("MimeType", string.Format("Only {0} mime type upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureMimeType), fileName));

            //    if (!_setting.PictureExtensionTypes.Contains(extension))
            //        ModelState.AddModelError("Extension", string.Format("Only {0} upload. Incorrect file name: {1}", string.Join(", ", _setting.PictureExtensionTypes), fileName));

            //    var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
            //    var path = Path.Combine(Server.MapPath("~/Content/Gallery/"), pictureName + extension);

            //    try
            //    {
            //        if (!ModelState.IsValid)
            //            return galleryDetail;

            //        Utility.FileUpload(picture, path);
            //        galleryDetail.PictureUrl = pictureName + extension;
            //    }
            //    catch (Exception ex)
            //    {
            //        ModelState.AddModelError("PictureUploadError", string.Format("Incorrect file name: {0} <br/> Error Detail: {1}", fileName, ex.Message));
            //    }

            //    return galleryDetail;
            //}).ToList();

            //if (!ModelState.IsValid)
            //    return View();

            //var db = _dbFactory();

            //if (string.IsNullOrEmpty(SequenceNumber))
            //    SequenceNumber = "99";

            //var gallery = new Gallery
            //{
            //    CategoryId = 0,
            //    CreateDate = DateTime.Now,
            //    Definition = Definition,
            //    Description = string.IsNullOrEmpty(Description) ? Name : Description,
            //    Hit = 0,
            //    Keyword = string.IsNullOrEmpty(Keyword) ? Name : Keyword,
            //    LanguageId = _setting.LanguageId,
            //    LanguageTag = _setting.Language,
            //    Name = Name,
            //    SequenceNumber = Convert.ToInt32(SequenceNumber),
            //    Status = Status,
            //    Title = string.IsNullOrEmpty(Title) ? Name : Title,
            //    Url = string.IsNullOrEmpty(Url) ? Utility.UrlSeo(Name) : Utility.UrlSeo(Url)
            //};

            //db.Galleries.Add(gallery);
            //db.SaveChanges();

            //var galleryId = gallery.Id;

            //images.ForEach(a => a.GalleryId = galleryId);

            //db.GalleryDetails.AddRange(images);
            //db.SaveChanges();
            #endregion
        }

        public void GetCategories(string language)
        {
            var db = _dbFactory();
            var menus = db.Categories.Where(a => a.LanguageTag == language).ToList();
            var treeView = Utility.CreateTree(menus).ToList();
            TempData[language] = treeView;
        }
    }
}