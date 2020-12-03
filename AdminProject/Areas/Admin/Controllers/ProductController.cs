using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Newtonsoft.Json;

namespace AdminProject.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMeasureService _measureService;
        private readonly ICategoryService _categoryService;
        private readonly IPictureService _pictureService;
        private readonly IPropertyService _propertyService;
        private readonly IBrandService _brandService;

        public ProductController(RuntimeSettings setting, IProductService productService, IMeasureService measureService,
            ICategoryService categoryService, IPictureService pictureService, IPropertyService propertyService, IBrandService brandService) : base(setting)
        {
            _productService = productService;
            _measureService = measureService;
            _categoryService = categoryService;
            _pictureService = pictureService;
            _propertyService = propertyService;
            _brandService = brandService;
        }

        public ActionResult Add()
        {
            SetPageHeader("Product", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);
            ViewBag.ProductList = DropdownTypes.GetProductType(ProductTypes.Normal);
            ViewBag.ProductGroupList = DropdownTypes.GetProductGroupType(ProductGroupTypes.Normal);
            ViewBag.StockList = DropdownTypes.GetStockType(StockTypes.InStock);
            ViewBag.Measures = _measureService.ActiveMeasureList();
            ViewBag.PropertyList = _propertyService.GetActivePropertyAndDetails();
            ViewBag.BrandList = _brandService.BrandList("1");

            GetCategories();

            var product = new Product
            {
                CreateDate = DateTime.Now,
                IsKdv = false,
                DiscountOdd = 0,
                KdvOdd = 18,
                StockNr = 20,
                Price = 0,
                Status = StatusTypes.Active
            };

            return View(GetDefaultProduct());
        }

        private Product GetDefaultProduct()
        {
            var product = new Product
            {
                CreateDate = DateTime.Now,
                IsKdv = false,
                DiscountOdd = 0,
                KdvOdd = 18,
                StockNr = 20,
                Price = 0,
                Status = StatusTypes.Active
            };
            return product;
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(string Code, string Name, string Description, ProductTypes ProductType, int BrandId,
            StockTypes StockType, string Price, string PurchasePrice, string SeoKeyword, string SeoDescription, bool IsKdv, StatusTypes Status, ProductGroupTypes GroupType, string[] ProductCategory,
            HttpPostedFileBase[] PictureUpload, string showcase, string[] measureId, string[] PropertyName, string[] PropertyValue, int DiscountOdd = 0, int KdvOdd = 0,
            int StockNr = 0)
        {
            SetPageHeader("Product", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ProductList = DropdownTypes.GetProductType(ProductType);
            ViewBag.ProductGroupList = DropdownTypes.GetProductGroupType(GroupType);
            ViewBag.StockList = DropdownTypes.GetStockType(StockType);
            ViewBag.Measures = _measureService.ActiveMeasureList();
            ViewBag.PropertyList = _propertyService.GetActivePropertyAndDetails();
            ViewBag.BrandList = _brandService.BrandList(BrandId.ToString());

            Price = Price.Replace(".", ",");
            PurchasePrice = PurchasePrice.Replace(".", ",");

            var ci = new CultureInfo("tr-TR");
            const NumberStyles decimalStyles = NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint;

            if (string.IsNullOrEmpty(Code))
                ModelState.AddModelError("Code", "Code is required.");

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Description))
                ModelState.AddModelError("Description", "Description is required.");

            if (string.IsNullOrEmpty(Price))
                ModelState.AddModelError("Price", "Price is required.");

            if (ProductCategory.All(string.IsNullOrEmpty))
                ModelState.AddModelError("Category", "Category is required.");

            decimal price, purchasePrice;

            if (!decimal.TryParse(Price, decimalStyles, ci, out price))
                ModelState.AddModelError("Price", "Price is format error.");

            if (price < 0)
                ModelState.AddModelError("Price", "You can not enter a number less than 0 TL.");

            if (!decimal.TryParse(PurchasePrice, decimalStyles, ci, out purchasePrice))
                ModelState.AddModelError("PurchasePrice", "Purchase Price is format error.");

            if (purchasePrice < 0)
                ModelState.AddModelError("PurchasePrice", "You can not enter a number less than 0 TL.");

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View();
            }

            var propertyList = new List<ProductPropertyModel>();
            if (PropertyValue.Any())
            {
                for (var i = 0; i < PropertyValue.Length; i++)
                {
                    var name = PropertyName[i];
                    var value = PropertyValue[i];

                    if (string.IsNullOrEmpty(value))
                        continue;

                    var model = new ProductPropertyModel
                    {
                        PropertyName = name,
                        PropertyValue = value
                    };

                    propertyList.Add(model);
                }
            }

            var product = new Product
            {
                BrandId = BrandId,
                Code = Code,
                CreateDate = DateTime.Now,
                Description = Description,
                DiscountOdd = DiscountOdd,
                IsKdv = IsKdv,
                KdvOdd = KdvOdd,
                Name = Name,
                SeoDescription = SeoDescription,
                SeoKeyword = SeoKeyword,
                PluralHit = 0,
                Price = price,
                PurchasePrice = purchasePrice,
                ProductType = ProductType,
                SingleHit = 0,
                Status = Status,
                StockNr = StockNr,
                StockType = StockType,
                UpdateDate = new DateTime(1900, 1, 1),
                GroupType = GroupType,
                Properties = _productService.PropertySerialization(propertyList),
                Url = Utility.UrlSeo($"{Name}-{DateTime.Now.Ticks}")
            };

            _productService.Add(product);

            for (var i = 0; i < PictureUpload.Length; i++)
            {
                try
                {
                    _pictureService.Add(product.Id, PictureUpload[i], i == showcase.ToInt32());
                }
                catch (CustomException ce)
                {
                    ModelState.AddModelError("PictureSave", ce.Message);
                }
            }

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View();
            }

            var measureIds = measureId.Where(a => !string.IsNullOrEmpty(a)).Select(a => a.ToInt32()).ToArray();
            _productService.AddProductMeasureAssg(product.Id, measureIds);

            var categoryIds = ProductCategory.Where(a => !string.IsNullOrEmpty(a)).Select(a => a.ToInt32()).ToArray();
            _productService.AddProductCategoryAssg(product.Id, categoryIds);

            GetCategories();

            Added();

            return View(GetDefaultProduct());
        }

        public ActionResult Edit(int id)
        {
            SetPageHeader("Product", "Edit");

            var product = _productService.GetProduct(id);

            ViewBag.BrandList = _brandService.BrandList(product.BrandId.ToString());
            ViewBag.ProductGroupList = DropdownTypes.GetProductGroupType(product.GroupType);
            ViewBag.StatusList = DropdownTypes.GetStatus(product.Status);
            ViewBag.ProductList = DropdownTypes.GetProductType(product.ProductType);
            ViewBag.StockList = DropdownTypes.GetStockType(product.StockType);
            ViewBag.Measures = _measureService.ActiveMeasureList();
            ViewBag.PropertyList = _propertyService.GetActivePropertyAndDetails();

            var productPictures = _pictureService.GetProductPicture(id);
            var productMeasures = string.Join(",", _productService.GetProductMeasures(id).Select(a => a.Id).ToArray());
            var productCategory = string.Join(",", _productService.GetProductCategories(id).Select(a => a.CategoryId).ToArray());

            ViewBag.ProductPictures = productPictures;
            ViewBag.ProductMeasures = productMeasures;
            ViewBag.ProductCategories = productCategory;
            ViewBag.ProductProperties = _productService.PropertyDeserialization(product.Properties);

            GetCategories();

            return View(product);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(int Id, string Code, string Name, string Description, string SeoKeyword, string SeoDescription, ProductTypes ProductType, ProductGroupTypes GroupType, int BrandId,
            StockTypes StockType, string Price, string PurchasePrice, bool IsKdv, StatusTypes Status, string[] ProductCategory,
            HttpPostedFileBase[] PictureUpload, string showcase, string[] measureId, int[] EditPictureId, string[] PropertyName, string[] PropertyValue, int DiscountOdd = 0, int KdvOdd = 0,
            int StockNr = 0)
        {
            SetPageHeader("Product", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ProductList = DropdownTypes.GetProductType(ProductType);
            ViewBag.StockList = DropdownTypes.GetStockType(StockType);
            ViewBag.Measures = _measureService.ActiveMeasureList();
            ViewBag.ProductGroupList = DropdownTypes.GetProductGroupType(GroupType);
            ViewBag.PropertyList = _propertyService.GetActivePropertyAndDetails();
            ViewBag.BrandList = _brandService.BrandList(BrandId.ToString());

            Price = Price.Replace(".", ",");
            PurchasePrice = PurchasePrice.Replace(".", ",");

            var ci = new CultureInfo("tr-TR");
            const NumberStyles decimalStyles = NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint;

            if (string.IsNullOrEmpty(Code))
                ModelState.AddModelError("Code", "Code is required.");

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(Description))
                ModelState.AddModelError("Description", "Description is required.");

            if (string.IsNullOrEmpty(Price))
                ModelState.AddModelError("Price", "Price is required.");

            if (ProductCategory.All(string.IsNullOrEmpty))
                ModelState.AddModelError("Category", "Category is required.");

            decimal price, purchasePrice;

            if (!decimal.TryParse(Price, decimalStyles, ci, out price))
                ModelState.AddModelError("Price", "Price is format error.");

            if (price < 0)
                ModelState.AddModelError("Price", "You can not enter a price less than 0 TL.");

            if (!decimal.TryParse(PurchasePrice, decimalStyles, ci, out purchasePrice))
                ModelState.AddModelError("PurchasePrice", "Purchase price is format error.");

            if (purchasePrice < 0)
                ModelState.AddModelError("PurchasePrice", "You can not enter a purchase price less than 0 TL.");

            var product = _productService.GetProduct(Id);
            ViewBag.ProductProperties = _productService.PropertyDeserialization(product.Properties);

            if (!ModelState.IsValid)
            {

                GetCategories();
                return View(product);
            }

            var propertyList = new List<ProductPropertyModel>();
            if (PropertyValue.Any())
            {
                for (var i = 0; i < PropertyValue.Length; i++)
                {
                    var name = PropertyName[i];
                    var value = PropertyValue[i];

                    if (string.IsNullOrEmpty(value))
                        continue;

                    var model = new ProductPropertyModel
                    {
                        PropertyName = name,
                        PropertyValue = value
                    };

                    propertyList.Add(model);
                }
            }

            product.BrandId = BrandId;
            product.GroupType = GroupType;
            product.Code = Code;
            product.Description = Description;
            product.DiscountOdd = DiscountOdd;
            product.IsKdv = IsKdv;
            product.KdvOdd = KdvOdd;
            product.Name = Name;
            product.SeoKeyword = SeoKeyword;
            product.SeoDescription = SeoDescription;
            product.Price = price;
            product.ProductType = ProductType;
            product.PurchasePrice = purchasePrice;
            product.Status = Status;
            product.StockNr = StockNr;
            product.StockType = StockType;
            product.UpdateDate = DateTime.Now;
            product.Properties = _productService.PropertySerialization(propertyList);

            _productService.Edit(Id, product);

            //images
            var oldImages = _pictureService.GetProductPicture(Id);

            //delete
            if (EditPictureId != null)
            {
                var deletedImages = oldImages.Where(a => !EditPictureId.Contains(a.Id) && a.ProductId == Id).ToList();
                deletedImages.ForEach(a =>
                {
                    var pictureId = a.Id;

                    _pictureService.DeletePicture(pictureId);
                });
            }
            //edit
            if (showcase == null)
                showcase = "1";

            if (showcase.Contains("old-"))
                _pictureService.ChangeShowcase(Id, showcase.Replace("old-", "").ToInt32());

            //add new images
            if (PictureUpload[0] != null)
                for (var i = 0; i < PictureUpload.Length; i++)
                {
                    try
                    {
                        _pictureService.Add(product.Id, PictureUpload[i], !showcase.Contains("old-") && i == showcase.ToInt32());
                    }
                    catch (CustomException ce)
                    {
                        ModelState.AddModelError("PictureSave", ce.Message);
                    }
                }

            if (!ModelState.IsValid)
            {
                GetCategories();
                return View(product);
            }

            //measures
            var allMeasureIds = measureId
                .Where(a => !string.IsNullOrEmpty(a))
                .Select(a => a.ToInt32())
                .ToArray();

            var oldMeasureIds = _productService.GetProductMeasures(Id).Select(a => a.Id).ToArray();

            var newMeasureIds = allMeasureIds.Where(a => !oldMeasureIds.Contains(a)).ToArray();
            var deleteMeasureIds = oldMeasureIds.Where(a => !allMeasureIds.Contains(a)).ToArray();

            _productService.DeleteProductMeasureAssg(product.Id, deleteMeasureIds);
            _productService.AddProductMeasureAssg(product.Id, newMeasureIds);

            //category
            var allCategoryIds = ProductCategory
                .Where(a => !string.IsNullOrEmpty(a))
                .Select(a => a.ToInt32())
                .ToArray();

            var oldCategoryIds = _productService.GetProductCategories(Id).Select(a => a.CategoryId).ToArray();

            var newCategoryIds = allCategoryIds.Where(a => !oldCategoryIds.Contains(a)).ToArray();
            var deleteCategoryIds = oldCategoryIds.Where(a => !allCategoryIds.Contains(a)).ToArray();

            _productService.DeleteProductCategoryAssg(Id, deleteCategoryIds);
            _productService.AddProductCategoryAssg(Id, newCategoryIds);

            Updated();

            return RedirectToAction("List");
        }

        public ActionResult List(
            string Code, 
            string Name,
            string Url, 
            ProductTypes ProductType = ProductTypes.All, 
            StockTypes StockType = StockTypes.All, 
            StatusTypes Status = StatusTypes.Active, 
            int Price = 0, 
            int Skip = 1, 
            int Take = 20)
        {
            SetPageHeader("Product", "List");

            ViewBag.TakeList = DropdownTypes.TakeCount(Take);
            ViewBag.StatusList = DropdownTypes.GetStatus(Status);
            ViewBag.ProductList = DropdownTypes.GetProductSearchType(ProductType);
            ViewBag.StockList = DropdownTypes.GetStockSearchType(StockType);
            ViewBag.ProductGroupList = DropdownTypes.GetProductGroupType(ProductGroupTypes.Normal);

            var request = new ProductSearchDto
            {
                Code = Code,
                Name = Name,
                Price = Price,
                ProductType = ProductType,
                Skip = Skip,
                Status = Status,
                StockType = StockType,
                Take = Take,
                Url = Url
            };

            var productList = _productService.ProductSearch(request);
            ViewBag.ProductPagerList = productList;
            
            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "Skip").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            ViewBag.UrlAddress = string.Format("/Admin/Product/List?{0}", nameValue.ToQueryString());

            return View(request);
        }

        public ActionResult Delete(int id)
        {
            _productService.DeleteProduct(id);

            return RedirectToAction("List");
        }

        private void GetCategories()
        {
            var categories = _categoryService.GetCategories(CategoryTypes.Product, StatusTypes.Active);
            ViewBag.Category = categories ?? Enumerable.Empty<MenuItem>();
        }
    }
}