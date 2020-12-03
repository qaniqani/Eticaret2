using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Newtonsoft.Json;
using PagedList;

namespace AdminProject.Services
{
    public class ProductService : IProductService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly IMeasureService _measureService;
        private readonly IPictureService _pictureService;
        private readonly ICommentService _commentService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ProductService(Func<AdminDbContext> dbFactory, IMeasureService measureService, IPictureService pictureService, ICommentService commentService, ICategoryService categoryService, IBrandService brandService)
        {
            _dbFactory = dbFactory;
            _measureService = measureService;
            _pictureService = pictureService;
            _commentService = commentService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        public void Add(Product product)
        {
            var db = _dbFactory();
            db.Products.Add(product);
            db.SaveChanges();
        }

        public void Edit(int id, Product productRequest)
        {
            var db = _dbFactory();
            var product = db.Products.FirstOrDefault(a => a.Id == id);
            if (product == null)
                throw new CustomException("Düzenlenecek ürün bulunamadı.");

            product.BrandId = productRequest.BrandId;
            product.GroupType = productRequest.GroupType;
            product.Code = productRequest.Code;
            product.CreateDate = productRequest.CreateDate;
            product.Description = productRequest.Description;
            product.DiscountOdd = productRequest.DiscountOdd;
            product.IsKdv = productRequest.IsKdv;
            product.KdvOdd = productRequest.KdvOdd;
            product.Name = productRequest.Name;
            product.Price = productRequest.Price;
            product.ProductType = productRequest.ProductType;
            product.Status = productRequest.Status;
            product.StockNr = productRequest.StockNr;
            product.StockType = productRequest.StockType;
            product.Url = productRequest.Url;
            product.UpdateDate = DateTime.Now;
            db.SaveChanges();
        }

        public Product GetProduct(int id)
        {
            var db = _dbFactory();
            var product = db.Products.FirstOrDefault(a => a.Id == id);
            return product;
        }

        public void DeleteProduct(int id)
        {
            var db = _dbFactory();

            var product = db.Products.FirstOrDefault(a => a.Id == id);
            var categories = db.CategoryProductAssgs.Where(a => a.ProductId == id).ToList();
            var measures = db.ProductMeasureAssgs.Where(a => a.ProductId == id).ToList();

            db.CategoryProductAssgs.RemoveRange(categories);
            db.ProductMeasureAssgs.RemoveRange(measures);
            db.Products.Remove(product);

            db.SaveChanges();

            _pictureService.DeleteAllPicture(id);
        }

        public List<Product> GetProduct(int[] id)
        {
            var db = _dbFactory();
            var product = db.Products.Where(a => id.Contains(a.Id)).ToList();
            return product;
        }

        public Product GetProduct(string url)
        {
            var db = _dbFactory();
            var product = db.Products.FirstOrDefault(a => a.Url == url && a.Status == StatusTypes.Active);
            return product;
        }

        public List<ProductViewDto> GetProducts(int[] id, ProductOrderTypes order)
        {
            var db = _dbFactory();
            var items = (from product in db.Products
                         join brand in db.Brands
                         on product.BrandId equals brand.Id
                         join picture in db.Pictures
                         on product.Id equals picture.ProductId
                         where picture.IsShowcase
                         && product.Status == StatusTypes.Active
                         && id.Contains(product.Id)
                         orderby product.Id descending
                         select new ProductViewDto
                         {
                             Id = product.Id,
                             StockType = product.StockType,
                             BigPicture = picture.BigPicture,
                             BrandName = brand.Name,
                             DiscountOdd = product.DiscountOdd,
                             IsDiscountApplied = product.DiscountOdd != 0,
                             Name = product.Name,
                             MinPicture = picture.MinPicture,
                             Price = product.Price,
                             Url = product.Url,
                             Hit = product.PluralHit
                         });

            if (order == ProductOrderTypes.Popularity)
                items = items.OrderByDescending(a => a.Hit);

            if (order == ProductOrderTypes.PriceDesc)
                items = items.OrderByDescending(a => a.Price);

            if (order == ProductOrderTypes.PriceAsc)
                items = items.OrderBy(a => a.Price);

            var result = items.ToList();

            result.ForEach(item =>
            {
                item.DiscountAmount = Function.ProductDiscount(item.Price, item.DiscountOdd);
                item.DiscountedPrice = item.Price - item.DiscountAmount;
            });

            return result;
        }

        public PagerList<Product> AllProductList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var products = db.Products.OrderBy(a => a.Code).Skip(skip).Take(take);
            var productCount = db.Products.Count();

            var result = new PagerList<Product>
            {
                TotalCount = productCount,
                List = products.ToList()
            };

            return result;
        }

        public PagerList<Product> ActiveProductList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var products = db.Products.Where(a => a.Status == StatusTypes.Active).Skip(skip).Take(take);
            var productCount = db.Products.Count();

            var result = new PagerList<Product>
            {
                TotalCount = productCount,
                List = products.ToList()
            };

            return result;
        }

        public PagerList<Product> ProductSearch(ProductSearchDto searchParam)
        {
            var db = _dbFactory();
            searchParam.Skip = (searchParam.Skip - 1) * searchParam.Take;
            var products = db.Products.Where(a => a.Status == searchParam.Status);

            if (!string.IsNullOrEmpty(searchParam.Code))
            {
                products = products.Where(a => a.Code == searchParam.Code);
                var codeTotalCount = products.Count();
                var codeSearchResult = products.OrderBy(a => a.CreateDate).Skip(searchParam.Skip).Take(searchParam.Take).ToList();
                var codeResult = new PagerList<Product>
                {
                    TotalCount = codeTotalCount,
                    List = codeSearchResult
                };

                return codeResult;
            }

            if (!string.IsNullOrEmpty(searchParam.Name))
                products = products.Where(a => a.Name.Contains(searchParam.Name));

            if (!string.IsNullOrEmpty(searchParam.Url))
                products = products.Where(a => a.Name.Contains(searchParam.Url));

            if (searchParam.StockType != StockTypes.All)
                products = products.Where(a => a.StockType == searchParam.StockType);

            if (searchParam.ProductType != ProductTypes.All)
                products = products.Where(a => a.ProductType == searchParam.ProductType);

            if (searchParam.Price > 0)
                products = products.Where(a => a.Price >= searchParam.Price);

            var totalCount = products.Count();
            var searchResult = products.OrderBy(a => a.CreateDate).Skip(searchParam.Skip).Take(searchParam.Take).ToList();

            var result = new PagerList<Product>
            {
                TotalCount = totalCount,
                List = searchResult
            };

            return result;
        }

        public void AddProductCategoryAssg(int productId, int[] categoryId)
        {
            if (categoryId.Length < 1)
                return;

            var db = _dbFactory();
            var productCategory = categoryId.Select(a => new CategoryProductAssg
            {
                CategoryId = a,
                ProductId = productId
            });

            db.CategoryProductAssgs.AddRange(productCategory);
            db.SaveChanges();
        }

        public void DeleteProductCategoryAssg(int productId, int[] categoryId)
        {
            if (categoryId.Length < 1)
                return;

            var db = _dbFactory();
            var productCategory = db.CategoryProductAssgs.Where(a => categoryId.Contains(a.CategoryId)).ToList();

            db.CategoryProductAssgs.RemoveRange(productCategory);
            db.SaveChanges();
        }

        public void EditProductCategoryAssg(int productId, int[] categoryId)
        {
            var db = _dbFactory();
            var oldCategory = db.CategoryProductAssgs.Where(a => a.ProductId == productId).ToList();
            db.CategoryProductAssgs.RemoveRange(oldCategory);
            db.SaveChanges();

            var productCategory = categoryId.Select(a => new CategoryProductAssg
            {
                CategoryId = a,
                ProductId = productId
            });

            db.CategoryProductAssgs.AddRange(productCategory);
            db.SaveChanges();
        }

        public List<CategoryProductAssg> GetProductCategories(int productId)
        {
            var db = _dbFactory();
            var categories = db.CategoryProductAssgs.Where(a => a.ProductId == productId).ToList();
            return categories;
        }

        public List<CategoryProductAssg> GetCategoryProducts(int categoryId)
        {
            var db = _dbFactory();
            var categories = db.CategoryProductAssgs.Where(a => a.CategoryId == categoryId).ToList();
            return categories;
        }

        public void AddProductMeasureAssg(int productId, int[] measureId)
        {
            if (measureId.Length < 1)
                return;

            var db = _dbFactory();
            var productMeasure = measureId.Select(a => new ProductMeasureAssg
            {
                MeasureId = a,
                ProductId = productId
            });

            db.ProductMeasureAssgs.AddRange(productMeasure);
            db.SaveChanges();
        }

        public void EditProductMeasureAssg(int productId, int[] measureId)
        {
            var db = _dbFactory();
            var measures = db.ProductMeasureAssgs.Where(a => a.ProductId == productId).ToList();
            db.ProductMeasureAssgs.RemoveRange(measures);
            db.SaveChanges();

            var productMeasure = measureId.Select(a => new ProductMeasureAssg
            {
                MeasureId = a,
                ProductId = productId
            });

            db.ProductMeasureAssgs.AddRange(productMeasure);
            db.SaveChanges();
        }

        public void DeleteProductMeasureAssg(int productId, int[] measureId)
        {
            if (measureId.Length < 1)
                return;

            var db = _dbFactory();
            var productMeasures = db.ProductMeasureAssgs.Where(a => measureId.Contains(a.MeasureId)).ToList();

            db.ProductMeasureAssgs.RemoveRange(productMeasures);
            db.SaveChanges();
        }

        public List<Measure> GetProductMeasures(int productId)
        {
            var db = _dbFactory();
            var measures = (from measureId in db.ProductMeasureAssgs
                            join measure in db.Measures
                            on measureId.MeasureId equals measure.Id
                            where measureId.ProductId == productId
                            && measure.Status == StatusTypes.Active
                            select measure).ToList();

            return measures;
        }

        public string PropertySerialization(List<ProductPropertyModel> propertyList)
        {
            return JsonConvert.SerializeObject(propertyList);
        }

        public List<ProductPropertyModel> PropertyDeserialization(string propertyList)
        {
            return JsonConvert.DeserializeObject<List<ProductPropertyModel>>(propertyList);
        }

        //Site Interface
        public List<ProductViewDto> GetFeaturedItems()
        {
            var db = _dbFactory();
            var items = (from product in db.Products
                         join brand in db.Brands
                         on product.BrandId equals brand.Id
                         join picture in db.Pictures
                         on product.Id equals picture.ProductId
                         where picture.IsShowcase
                         && product.Status == StatusTypes.Active
                         && product.GroupType == ProductGroupTypes.WeChose
                         orderby Guid.NewGuid()
                         select new ProductViewDto
                         {
                             Id = product.Id,
                             StockType = product.StockType,
                             BigPicture = picture.BigPicture,
                             BrandName = brand.Name,
                             DiscountOdd = product.DiscountOdd,
                             IsDiscountApplied = product.DiscountOdd != 0,
                             Name = product.Name,
                             MinPicture = picture.MinPicture, // /Content/Product/
                             Price = product.Price,
                             Url = product.Url
                         }).Take(8).ToList();

            items.ForEach(item =>
            {
                item.DiscountAmount = Function.ProductDiscount(item.Price, item.DiscountOdd);
                item.DiscountedPrice = item.Price - item.DiscountAmount;
            });

            return items;
        }

        public List<ProductViewDto> GetTopRatedItems()
        {
            var db = _dbFactory();
            var items = (from product in db.Products
                         join brand in db.Brands
                         on product.BrandId equals brand.Id
                         join picture in db.Pictures
                         on product.Id equals picture.ProductId
                         where picture.IsShowcase
                         && product.Status == StatusTypes.Active
                         && product.GroupType == ProductGroupTypes.Liked
                         orderby Guid.NewGuid()
                         select new ProductViewDto
                         {
                             Id = product.Id,
                             StockType = product.StockType,
                             BigPicture = picture.BigPicture,
                             BrandName = brand.Name,
                             DiscountOdd = product.DiscountOdd,
                             IsDiscountApplied = product.DiscountOdd != 0,
                             Name = product.Name,
                             MinPicture = picture.MinPicture,
                             Price = product.Price,
                             Url = product.Url
                         }).Take(8).ToList();

            items.ForEach(item =>
            {
                item.DiscountAmount = Function.ProductDiscount(item.Price, item.DiscountOdd);
                item.DiscountedPrice = item.Price - item.DiscountAmount;
            });

            return items;
        }

        public List<ProductViewDto> GetLastAddedItems()
        {
            var db = _dbFactory();
            var items = (from product in db.Products
                         join brand in db.Brands
                         on product.BrandId equals brand.Id
                         join picture in db.Pictures
                         on product.Id equals picture.ProductId
                         where picture.IsShowcase
                         && product.Status == StatusTypes.Active
                         orderby product.CreateDate descending
                         select new ProductViewDto
                         {
                             Id = product.Id,
                             StockType = product.StockType,
                             BigPicture = picture.BigPicture,
                             BrandName = brand.Name,
                             DiscountOdd = product.DiscountOdd,
                             IsDiscountApplied = product.DiscountOdd != 0,
                             Name = product.Name,
                             MinPicture = picture.MinPicture, // /Content/Product/
                             Price = product.Price,
                             Url = product.Url
                         }).Take(8).ToList();

            items.ForEach(item =>
            {
                item.DiscountAmount = Function.ProductDiscount(item.Price, item.DiscountOdd);
                item.DiscountedPrice = item.Price - item.DiscountAmount;
            });

            return items;
        }

        public List<ProductViewDto> GetLastViewItems()
        {
            var db = _dbFactory();
            var items = (from product in db.Products
                         join brand in db.Brands
                         on product.BrandId equals brand.Id
                         join picture in db.Pictures
                         on product.Id equals picture.ProductId
                         where picture.IsShowcase
                         && product.Status == StatusTypes.Active
                         orderby Guid.NewGuid()
                         select new ProductViewDto
                         {
                             Id = product.Id,
                             StockType = product.StockType,
                             BigPicture = picture.BigPicture,
                             BrandName = brand.Name,
                             DiscountOdd = product.DiscountOdd,
                             IsDiscountApplied = product.DiscountOdd != 0,
                             Name = product.Name,
                             MinPicture = picture.MinPicture, // /Content/Product/
                             Price = product.Price,
                             Url = product.Url
                         }).Take(8).ToList();

            items.ForEach(item =>
            {
                item.DiscountAmount = Function.ProductDiscount(item.Price, item.DiscountOdd);
                item.DiscountedPrice = item.Price - item.DiscountAmount;
            });

            return items;
        }

        public ProductDetailDto GetProductDetail(string url)
        {
            var product = GetProduct(url);
            if (product == null)
                throw new CustomException("Ürün bulunamadı.");

            var productView = new ProductDetailViewDto
            {
                BrandId = product.BrandId,
                BrandName = _brandService.GetBrand(product.BrandId).Name,
                Code = product.Code,
                DateTime = product.CreateDate,
                Description = product.Description,
                SeoDescription = product.SeoDescription,
                SeoKeyword = product.SeoKeyword,
                DiscountOdd = product.DiscountOdd,
                DiscountAmount = Function.ProductDiscount(product.Price, product.DiscountOdd),
                DiscountPrice = product.Price - Function.ProductDiscount(product.Price, product.DiscountOdd),
                IsKdv = product.IsKdv,
                KdvAmount = !product.IsKdv ? Function.KdvAmount(product.Price, product.KdvOdd) : Function.KdvAmountProductDown(product.Price, product.KdvOdd),
                KdvOdd = product.KdvOdd,
                Name = product.Name,
                Id = product.Id,
                ProductPrice = product.Price,
                ProductType = product.ProductType,
                StockType = product.StockType,
                ProductTypeText = Utility.AllProductConvert[product.ProductType],
                StockTypeText = Utility.AllStockConvert[product.StockType],
                IsDiscountApplied = product.DiscountOdd != 0,
                Properties = PropertyDeserialization(product.Properties)
            };

            var productDto = new ProductDetailDto
            {
                Product = productView
            };

            var measures = GetProductMeasures(product.Id).Select(item => new MeasureListDto
            {
                MeasureName = item.Name,
                Items = _measureService.ActiveMeasureDetailList(item.Id).Select(a => new ListItem
                {
                    Text = a.Size,
                    Value = a.Id.ToString()
                }).ToList()
            }).ToList();
            productDto.Measures = measures;

            var pictures = _pictureService.GetProductPicture(product.Id).Select(item => new PictureItemDto
            {
                BigPicture = item.BigPicture,
                IsShowcase = item.IsShowcase,
                MinPicture = item.MinPicture
            }).OrderByDescending(a => a.IsShowcase).ToList();
            productDto.Pictures = pictures;

            var comments = _commentService.GetProductComment(product.Id).Select(item => new ProductCommentDto
            {
                Comment = item.Comment.Detail,
                DateTime = item.Comment.DateTime.ToString("dd.MM.yyyy HH:mm:ss"),
                UserNameSurname = item.Name
            }).ToList();
            productDto.Comments = comments;

            //var productProperties = 

            var productCategoryIds = GetProductCategories(product.Id).Select(a => a.CategoryId).ToArray();
            var categories = _categoryService.GetChainCategoryLink(productCategoryIds, ">");
            productDto.Categories = categories;

            return productDto;
        }

        public PagerList<ProductViewDto> GetCategoryProduct1(string categoryUrl, int skip, int take, ProductOrderTypes order)
        {
            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
                return new PagerList<ProductViewDto>
                {
                    TotalCount = 0,
                    List = new List<ProductViewDto>()
                };

            skip = (skip - 1) * take;

            var catId = category.Id;
            var productIds = GetCategoryProducts(catId).Select(a => a.ProductId).ToArray();

            var products = GetProducts(productIds, order);
            var count = products.Count;
            var productList = products.Skip(skip).Take(take).ToList();

            var result = new PagerList<ProductViewDto>
            {
                TotalCount = count,
                List = productList
            };

            return result;
        }

        public StaticPagedList<ProductViewDto> GetCategoryProduct(string categoryUrl, int skip, int take, ProductOrderTypes order)
        {
            var category = _categoryService.GetCategory(categoryUrl);
            if (category == null)
                return null;

            var catId = category.Id;
            var productIds = GetCategoryProducts(catId).Select(a => a.ProductId).ToArray();

            var products = GetProducts(productIds, order);
            var productMeta = products.ToPagedList(skip, take);

            var result = new StaticPagedList<ProductViewDto>(productMeta, productMeta);

            return result;
        }

        public StaticPagedList<ProductViewDto> GetSearchProduct(string key, int skip, int take, ProductOrderTypes order)
        {
            var keys = key.Split(' ').ToList();

            var db = _dbFactory();

            var query = "Select Id From Product Where Status = 1 And ({0})";
            var where = string.Empty;

            for (var i = 0; i < keys.Count; i++)
            {
                var a = keys[i];

                var n = $"(Name Like ('%{a}%') or Description Like ('%{a}%') or SeoDescription Like ('%{a}%') or SeoKeyword Like ('%{a}%') or Url Like ('%{a}%'))";

                if (i != keys.Count - 1)
                    n += " or ";

                where += where + n;
            }

            query = string.Format(query, where);

            var productIds = db.Database.SqlQuery<int>(query).ToArray();

            var products = GetProducts(productIds, order);
            var productMeta = products.ToPagedList(skip, take);

            var result = new StaticPagedList<ProductViewDto>(productMeta, productMeta);

            return result;
        }

        public List<ProductParentCategoryItemDto> GetParentCategoryProduct(int categoryId)
        {
            var db = _dbFactory();
            var parentIds = _categoryService.GetCategoryParentIds(categoryId);

            if (parentIds.Length <= 0)
                return new List<ProductParentCategoryItemDto>();

            var productCounts = (from a in db.CategoryProductAssgs
                join p in db.Products
                on a.ProductId equals p.Id
                join c in db.Categories
                on a.CategoryId equals c.Id
                where p.Status == StatusTypes.Active
                && parentIds.Contains(a.CategoryId)
                group new {a.ProductId, p.Id} by new {a.CategoryId, c.Url, c.Name, c.Picture} into grp
                select new ProductParentCategoryItemDto
                {
                    Picture = grp.Key.Picture,
                    Id = grp.Key.CategoryId,
                    Name = grp.Key.Name,
                    Url = grp.Key.Url,
                    ProductCount = grp.Count()
                }).ToList();

            return productCounts;
        }

        //RSS
        public List<Product> GetLast20Product()
        {
            var db = _dbFactory();
            var products = db.Products.OrderByDescending(a => a.Id).Where(a => a.Status == StatusTypes.Active).Take(20).ToList();

            //var result = products.Select(a => new RssItemModel
            //{
            //    Description = a.Description,
            //    Guid = $"/product/{a.Url}/detail",
            //    Link = $"/product/{a.Url}/detail",
            //    PublishDate = a.CreateDate,
            //    Title = a.Name
            //}).ToList();

            return products;
        }
    }
}