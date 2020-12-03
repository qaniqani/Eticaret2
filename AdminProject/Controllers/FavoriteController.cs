using System;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    [RoutePrefix("favorite")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;

        public FavoriteController(IFavoriteService favoriteService, IProductService productService)
        {
            _favoriteService = favoriteService;
            _productService = productService;
        }

        [Route("add")]
        public ActionResult Add(string url, string productName)
        {
            if (string.IsNullOrEmpty(url))
                ModelState.AddModelError("Url", "Ürün bulunamadı");

            if (string.IsNullOrEmpty(productName))
                ModelState.AddModelError("ProductName", "Ürün bulunamadı");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = string.Join(", ", ModelState.Values.SelectMany(a => a.Errors.Select(e => e.ErrorMessage)));
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            if (Utility.UserCheck() == null)
            {
                Response.StatusCode = 400;
                return Json("Lütfen favorilere eklemeden önce üye girişi yapınız.", JsonRequestBehavior.AllowGet);
            }

            var product = _productService.GetProduct(url);
            if(product == null)
            {
                Response.StatusCode = 400;
                return Json("Ürün bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            var favori = new Favorite
            {
                DateTime = DateTime.Now,
                ProductId = product.Id,
                ProductName = productName,
                ProductUrl = url,
                UserId = Utility.UserCheck().Id,
            };

            _favoriteService.Add(favori);

            return Json($"{productName} favorilerinize eklendi", JsonRequestBehavior.AllowGet);
        }
    }
}