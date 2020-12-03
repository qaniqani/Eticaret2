using System.Collections.Generic;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;
using PagedList;

namespace AdminProject.Services.Interface
{
    public interface IProductService
    {
        void Add(Product product);
        void Edit(int id, Product productRequest);
        Product GetProduct(int id);
        List<Product> GetProduct(int[] id);
        Product GetProduct(string url);
        PagerList<Product> AllProductList(int skip, int take);
        PagerList<Product> ActiveProductList(int skip, int take);
        PagerList<Product> ProductSearch(ProductSearchDto searchParam);
        void AddProductCategoryAssg(int productId, int[] categoryId);
        void EditProductCategoryAssg(int productId, int[] categoryId);
        void DeleteProductCategoryAssg(int productId, int[] categoryId);
        void AddProductMeasureAssg(int productId, int[] measureId);
        void EditProductMeasureAssg(int productId, int[] measureId);
        void DeleteProductMeasureAssg(int productId, int[] measureId);
        List<Measure> GetProductMeasures(int productId);
        List<CategoryProductAssg> GetProductCategories(int productId);
        string PropertySerialization(List<ProductPropertyModel> propertyList);
        List<ProductPropertyModel> PropertyDeserialization(string propertyList);
        void DeleteProduct(int id);

        //site interface
        List<ProductViewDto> GetFeaturedItems();
        List<ProductViewDto> GetTopRatedItems();
        List<ProductViewDto> GetLastViewItems();
        List<ProductViewDto> GetLastAddedItems();
        ProductDetailDto GetProductDetail(string url);
        List<ProductParentCategoryItemDto> GetParentCategoryProduct(int categoryId);
        //PagerList<ProductViewDto> GetCategoryProduct(string categoryUrl, int skip, int take, ProductOrderTypes order);
        StaticPagedList<ProductViewDto> GetCategoryProduct(string categoryUrl, int skip, int take, ProductOrderTypes order);
        StaticPagedList<ProductViewDto> GetSearchProduct(string key, int skip, int take, ProductOrderTypes order);

        List<Product> GetLast20Product();
    }
}