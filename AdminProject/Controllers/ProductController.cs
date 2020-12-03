using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    [RoutePrefix("product")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;

        public ProductController(IProductService productService, ICategoryService categoryService,
            ICommentService commentService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _commentService = commentService;
        }

        [Route("{url}/detail")]
        //[OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult ProductDetail(string url)
        {
            if (string.IsNullOrEmpty(url))
                return RedirectToRoutePermanent("/");

            try
            {
                var product = _productService.GetProductDetail(url);
                ViewBag.Url = url;
                ViewBag.Description = product.Product.SeoDescription;
                ViewBag.Keyword = product.Product.SeoKeyword;

                return View(product);
            }
            catch (CustomException cEx)
            {
                return Redirect("/404.html");
            }
        }

        [Route("{category}/list")]
        //[OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult ProductList(string category, int? page, ProductOrderTypes order = ProductOrderTypes.Normal, int take = 12)
        {
            ViewBag.ProductOrder = DropdownTypes.ProductOrder(order);
            ViewBag.ProductTakeCount = DropdownTypes.ProductTakeCount(take);
            ViewBag.CategoryUrl = category;

            if (string.IsNullOrEmpty(category))
                return RedirectToRoutePermanent("/");

            var cat = _categoryService.GetCategory(category);
            if (cat == null)
                return RedirectPermanent("/");

            if (page == null)
            {
                page = 1;
                return RedirectPermanent($"/product/{category}/list?page={page}");
            }

            var catId = cat.Id;
            ViewBag.ParentCategory = _categoryService.GetCategoryParentList(catId).Where(a => a.Status == StatusTypes.Active).OrderBy(a => a.SequenceNumber).ToList();

            ViewBag.Page = page;
            ViewBag.Take = take;
            ViewBag.Order = order;

            ViewBag.Category = cat;

            var product = _productService.GetCategoryProduct(category, Convert.ToInt32(page), take, order);
            ViewBag.Product = product;

            var parentCategories = _productService.GetParentCategoryProduct(catId);
            ViewBag.ParentCategories = parentCategories;

            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "page").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            var nameValueQuery = nameValue.ToQueryString();
            if(nameValueQuery.Any())
            {
                ViewBag.Asd = "ad";
                ViewBag.UrlAddress = $"/product/{cat.Url}/list?{nameValueQuery}";
            }
            else
                ViewBag.UrlAddress = $"/product/{cat.Url}/list";

            return View();
        }

        [Route("comment")]
        [CookieCheck]
        [AuthorizationFilter]
        [HttpPost]
        public ActionResult Comment(string productId, string message)
        {
            if (string.IsNullOrEmpty(productId))
                ModelState.AddModelError("ProductId", "Ürün seçiniz");

            if (string.IsNullOrEmpty(message))
                ModelState.AddModelError("Message", "Yorumu yazınız");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var userId = Utility.UserCheck().Id;

            var comment = new Comment
            {
                DateTime = DateTime.Now,
                Detail = message,
                ProductId = productId.ToInt32(),
                Status = CommentTypes.New,
                UserId = userId
            };

            _commentService.Add(comment);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult LastedViewItems()
        {
            var items = _productService.GetLastViewItems();

            return PartialView("LastedViewItems", items);
        }
    }
}