using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Models;
using File = AdminProject.Infrastructure.Models.File;

namespace AdminProject.Controllers
{
    public class FileController : BaseController
    {
        private readonly Func<AdminDbContext> _dbFactory;

        private readonly string[] _extensions = {   ".doc", ".docx", ".xls", ".xlsx", ".rar", ".zip", ".7z", ".bmp", ".jpg", 
                                                    ".jpeg", ".ico", ".png", ".3gp", ".avi", ".pdf", ".swf", ".apk", ".dxf", 
                                                    ".dwg", ".gif", ".h263", ".mpg", ".mpeg", ".mdb", ".xlam", ".xlsb", ".xltm", 
                                                    ".xlsm", ".pptx", ".ppt", ".dotx", ".onetoc", ".ppam", ".pptm", ".ppsm", ".wm", 
                                                    ".wma", ".xps", ".mpga", ".mp4", ".mp4a", ".ogv", ".oga", ".webm", ".weba",".odb", 
                                                    ".odg", ".odt", ".odm", ".otf", ".psd", ".rtf", ".rtx", ".vcf", ".xml", ".xslt" };

        public FileController(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("File", "Add New File/ Picture");

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, string Description, HttpPostedFileBase File)
        {
            SetPageHeader("File", "Add New File/ Picture");

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (File == null)
                ModelState.AddModelError("File", "File is required.");

            if (!ModelState.IsValid)
                return View();

            var file = new File
            {
                Description = Description,
                Hit = 0,
                Name = Name,
                Status = StatusTypes.Active,
                UploadDate = DateTime.Now,
                UploadUserId = Utility.SessionCheck().Id
            };

            if (File != null)
            {
                var fileName = File.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (_extensions.Contains(extension))
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}", Name, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(File, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("FileUploadError", ex.Message);
                            return View();
                        }

                        file.FileUrl = pictureName + extension;
                    }
                    else
                    {
                        ModelState.AddModelError("Extension",
                            string.Format("Only upload types: {0}", string.Join(",", _extensions)));
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

            if (file != null) return View(file);

            Warning();
            return RedirectToAction("List");

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
        }

        [HttpPost]
        public ActionResult Edit(int id, string Name, string Description, HttpPostedFileBase File)
        {
            SetPageHeader("File", "Edit File/ Picture");

            //ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (File == null)
                ModelState.AddModelError("File", "File is required.");

            if (!ModelState.IsValid)
                return View();

            var db = _dbFactory();

            var file = db.Files.FirstOrDefault(a => a.Id == id);
            if (file == null)
            {
                ModelState.AddModelError("FileNotFound", "File was not found.");
                return View();
            }

            file.Description = Description;
            file.Name = Name;
            file.Status = StatusTypes.Active;
            file.UploadDate = DateTime.Now;
            file.UploadUserId = Utility.SessionCheck().Id;

            if (File != null)
            {
                var fileName = File.FileName;
                var extension = Path.GetExtension(fileName);

                if (extension != null)
                {
                    if (_extensions.Contains(extension))
                    {
                        var pictureName = Utility.UrlSeo(string.Format("{0}-{1}-{2}", Name, fileName, DateTime.Now));
                        var path = Path.Combine(Server.MapPath("~/Content/File/"), pictureName + extension);
                        try
                        {
                            Utility.FileUpload(File, path);
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("FileUploadError", ex.Message);
                            return View(file);
                        }

                        file.FileUrl = pictureName + extension;
                    }
                    else
                    {
                        ModelState.AddModelError("Extension",
                            string.Format("Only upload types: {0}", string.Join(",", _extensions)));
                        return View(file);
                    }
                }
                else
                {
                    ModelState.AddModelError("Extension", "File extension not found.");
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
    }
}