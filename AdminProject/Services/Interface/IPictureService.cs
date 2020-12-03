using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IPictureService
    {
        void Add(int productId, HttpPostedFileBase picture, bool isShowcase);
        Picture GetPicture(int pictureId);
        Picture GetProductShowcasePicture(int productId);
        void ChangeShowcase(int productId, int showcasePictureId);
        List<Picture> GetProductPicture(int productId);
        void DeleteAllPicture(int productId);
        void DeletePicture(int pictureId);
        string SaveDefaultPicture(HttpPostedFileBase picture, string uploadPath);
        string GetMaxPictureNameFormat(string pictureName, string extension);
        string GetMinPictureNameFormat(string pictureName, string extension);
    }
}