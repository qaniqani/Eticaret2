using System;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using System.Collections.Specialized;
using System.Linq;

namespace AdminProject.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IProductService _productService;

        public SearchController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("search")]
        public ActionResult Index(string key, int? page, ProductOrderTypes order = ProductOrderTypes.Normal, int take = 12)
        {
            if (string.IsNullOrEmpty(key))
                return RedirectPermanent("/");

            if (key.Length < 3)
            {
                ErrorMessage("En az 3 karakter giriniz.");
                ViewBag.Product = new PagerList<ProductViewDto>();
                return View();
            }

            ViewBag.ProductOrder = DropdownTypes.ProductOrder(order);
            ViewBag.ProductTakeCount = DropdownTypes.ProductTakeCount(take);
            ViewBag.Key = key;

            if (page == null)
            {
                page = 1;
                return RedirectPermanent($"/search?key={key}&page={page}");
            }

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Order = order;

            var product = _productService.GetSearchProduct(key, Convert.ToInt32(page), take, order);
            ViewBag.Product = product;

            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "page").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            var nameValueQuery = nameValue.ToQueryString();
            if (nameValueQuery.Count() > 1)
            {
                ViewBag.Asd = "ad";
                ViewBag.UrlAddress = $"/search?{nameValueQuery}";
            }
            else
                ViewBag.UrlAddress = $"/search?key={key}";

            return View();
        }
    }
}