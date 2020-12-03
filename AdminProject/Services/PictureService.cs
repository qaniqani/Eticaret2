using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class PictureService : IPictureService
    {
        private readonly RuntimeSettings _setting;
        private readonly Func<AdminDbContext> _dbFactory;

        public PictureService(Func<AdminDbContext> dbFactory, RuntimeSettings setting)
        {
            _dbFactory = dbFactory;
            _setting = setting;
        }

        public void Add(int productId, HttpPostedFileBase picture, bool isShowcase)
        {
            var fileName = picture.FileName;
            var extension = Path.GetExtension(fileName);

            if (!_setting.PictureMimeType.Contains(picture.ContentType))
                throw new CustomException($"Sadece {string.Join(", ", _setting.PictureMimeType)} mime tipine ait resimler yüklenebilir.");

            if (!_setting.PictureExtensionTypes.Contains(extension))
                throw new CustomException($"Sadece {string.Join(", ", _setting.PictureExtensionTypes)} uzatısına sahip resimler yüklenebilir.");

            var pictureName = Utility.UrlSeo($"{fileName}-{DateTime.Now.Ticks}");

            var maxName = GetMaxPictureNameFormat(pictureName, extension);
            var minName = GetMinPictureNameFormat(pictureName, extension);

            var directoryMaxPath = Path.Combine(HttpContext.Current.Server.MapPath("~" + _setting.ProductImagePath),
                maxName);
            var directoryMinPath = Path.Combine(HttpContext.Current.Server.MapPath("~" + _setting.ProductImagePath),
                minName);

            Utility.FileUpload(picture, directoryMaxPath, _setting.ProductImageMaxWidth, _setting.ProductImageMaxHeight);
            Utility.FileUpload(picture, directoryMinPath, _setting.ProductImageMinWidth, _setting.ProductImageMinHeight);

            var db = _dbFactory();

            var maxPath = Path.Combine(_setting.ProductImagePath, maxName);
            var minPath = Path.Combine(_setting.ProductImagePath, minName);

            var p = new Picture
            {
                BigPicture = maxPath,
                ProductId = productId,
                MinPicture = minPath,
                IsShowcase = isShowcase
            };

            db.Pictures.Add(p);
            db.SaveChanges();
        }

        public Picture GetPicture(int pictureId)
        {
            var db = _dbFactory();
            var picture = db.Pictures.FirstOrDefault(a => a.Id == pictureId);
            return picture;
        }

        public Picture GetProductShowcasePicture(int productId)
        {
            var db = _dbFactory();
            var picture = db.Pictures.FirstOrDefault(a => a.ProductId == productId && a.IsShowcase) ??
                          db.Pictures.FirstOrDefault(a => a.ProductId == productId);

            return picture;
        }

        public void ChangeShowcase(int productId, int showcasePictureId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => a.ProductId == productId).ToList();
            pictures.ForEach(item => item.IsShowcase = false);
            var showcase = pictures.FirstOrDefault(a => a.Id == showcasePictureId);
            if (showcase == null)
                return;

            showcase.IsShowcase = true;
            db.SaveChanges();
        }

        public List<Picture> GetProductPicture(int productId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => a.ProductId == productId).ToList();
            return pictures;
        }

        public void DeleteAllPicture(int productId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.Where(a => a.ProductId == productId);
            db.Pictures.RemoveRange(pictures);
            db.SaveChanges();
        }

        public void DeletePicture(int pictureId)
        {
            var db = _dbFactory();
            var pictures = db.Pictures.FirstOrDefault(a => a.Id == pictureId);
            if (pictures == null)
                return;

            try
            {
                var minFi = new FileInfo(HttpContext.Current.Server.MapPath("~" + pictures.MinPicture));
                minFi.Delete();
            }catch{// ignored
            }

            try
            {
                var maxFi = new FileInfo(HttpContext.Current.Server.MapPath("~" + pictures.BigPicture));
                maxFi.Delete();
            }catch{// ignored
            }

            db.Pictures.Remove(pictures);
            db.SaveChanges();
        }

        public string SaveDefaultPicture(HttpPostedFileBase picture, string uploadPath)
        {
            var fileName = picture.FileName;
            var extension = Path.GetExtension(fileName);

            if (!_setting.PictureMimeType.Contains(picture.ContentType))
                throw new CustomException(string.Format("Sadece {0} mime tipleri yüklenebilir.", string.Join(", ", _setting.PictureMimeType)));

            if (!_setting.PictureExtensionTypes.Contains(extension))
                throw new CustomException("Extension", string.Format("Sadece {0} uzantılar yüklenebilir.", string.Join(", ", _setting.PictureExtensionTypes)));

            var pictureName = Utility.UrlSeo(string.Format("{0}-{1}", fileName, DateTime.Now.Ticks));
            var path = Path.Combine(HttpContext.Current.Server.MapPath("~" + uploadPath), pictureName + extension);

            Utility.FileUpload(picture, path, _setting.ImageMaxWidth, _setting.ImageMaxHeight);

            return pictureName + extension;
        }

        public string GetMaxPictureNameFormat(string pictureName, string extension)
        {
            return string.Format("{0}-{1}x{2}{3}", pictureName, _setting.ProductImageMaxWidth, _setting.ProductImageMaxHeight, extension);
        }

        public string GetMinPictureNameFormat(string pictureName, string extension)
        {
            return string.Format("{0}-{1}x{2}{3}", pictureName, _setting.ProductImageMinWidth, _setting.ProductImageMinHeight, extension);
        }
    }
}