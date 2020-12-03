using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Controllers
{
    [RoutePrefix("basket")]
    public class BasketController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public BasketController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }

        [Route("")]
        [CookieCheck]
        public ActionResult Index()
        {
            var sessionId = GetSessionId();
            var userId = 0;
            if (Utility.UserCheck() != null)
                userId = Utility.UserCheck().Id;

            var list = _basketService.GetTopBasketList(sessionId, userId);

            return View(list);
        }

        [Route("set-basket")]
        [HttpPost]
        [CookieCheck]
        public ActionResult SetBasketCount(int basketId, int count)
        {
            var sessionId = GetSessionId();
            var userId = 0;
            if (Utility.UserCheck() != null)
                userId = Utility.UserCheck().Id;

            if (userId == 0)
                _basketService.EditUnitChange(sessionId, basketId, count);
            else
                _basketService.EditUnitChange(userId, basketId, count);

            SuccessMessage("Sepetiniz güncellendi.");

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [Route("delete-basket")]
        [HttpPost]
        [CookieCheck]
        public ActionResult DeleteBasketItem(int basketId)
        {
            var sessionId = GetSessionId();
            var userId = 0;
            if (Utility.UserCheck() != null)
                userId = Utility.UserCheck().Id;

            if(userId != 0)
                _basketService.Delete(userId, basketId);
            else
                _basketService.Delete(sessionId, basketId);

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [Route("add")]
        [CookieCheck]
        public ActionResult Add(int? id, int? unit, string name, string[] measureIds, string[] measureNames)
        {
            if (id == null)
                ModelState.AddModelError("ProductId", "Ürün seçiniz");

            if (unit == null)
                ModelState.AddModelError("Unit", "Adet seçiniz");

            if(string.IsNullOrEmpty(name))
                ModelState.AddModelError("ProductName", "Ürün seçiniz");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = string.Join(", ", ModelState.Values.SelectMany(a => a.Errors.Select(e => e.ErrorMessage)));
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var productId = Convert.ToInt32(id);
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                Response.StatusCode = 400;
                return Json("Ürün bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            if(name != product.Name)
            {
                Response.StatusCode = 400;
                return Json("Ürün bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            var measureList = new List<ProductDetail>();
            if (measureIds != null)
            {
                for (var i = 0; i < measureIds.Length; i++)
                {
                    var measureId = measureIds[i];
                    var measureName = measureNames[i];
                    var detail = new ProductDetail
                    {
                        Name = measureName,
                        Value = measureId
                    };
                    measureList.Add(detail);
                }
            }

            var basket = new Basket
            {
                CargoId = 0,
                DateTime = DateTime.Now,
                IpAddress = GetIpAddress(),
                OtherDetail = Tool.ProductDetailSerialization(measureList),
                Price = product.Price,
                ProductId = productId,
                ProductName = product.Name,
                ProductUrl = product.Url,
                SessionId = GetSessionId(),
                Unit = Convert.ToInt32(unit),
                UserId = Utility.UserCheck() != null ? Utility.UserCheck().Id : 0
            };

            _basketService.AddOrChange(basket);

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [Route("list")]
        [CookieCheck]
        public ActionResult GetList()
        {
            var sessionId = GetSessionId();
            var userId = 0;
            if (Utility.UserCheck() != null)
                userId = Utility.UserCheck().Id;

            var list = _basketService.GetTopBasketList(sessionId, userId);

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}