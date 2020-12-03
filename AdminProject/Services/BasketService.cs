using System;
using System.Collections.Generic;
using System.Linq;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Newtonsoft.Json;

namespace AdminProject.Services
{
    public class BasketService : IBasketService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly ICampaignService _campaignService;
        private readonly ICargoService _cargoService;
        private readonly IOrderService _orderService;

        public BasketService(Func<AdminDbContext> dbFactory, ICampaignService campaignService, ICargoService cargoService, IOrderService orderService)
        {
            _dbFactory = dbFactory;
            _campaignService = campaignService;
            _cargoService = cargoService;
            _orderService = orderService;
        }

        public void Add(Basket basket)
        {
            var db = _dbFactory();
            db.Baskets.Add(basket);
            db.SaveChanges();
        }

        public void AddOrChange(Basket request)
        {
            var db = _dbFactory();

            var userId = request.UserId;
            var productId = request.ProductId;
            var sessionId = request.SessionId;

            if (userId != 0)
            {
                var basket = db.Baskets.FirstOrDefault(a => (a.SessionId == sessionId || a.UserId == userId) && a.ProductId == productId);
                if (basket == null)
                {
                    db.Baskets.Add(request);
                    db.SaveChanges();
                    return;
                }

                basket.Unit += request.Unit;
                basket.UserId = basket.UserId;
                basket.SessionId = basket.SessionId;
                db.SaveChanges();
            }
            else
            {
                var basket = db.Baskets.FirstOrDefault(a => a.SessionId == sessionId && a.ProductId == productId);
                if (basket != null)
                {
                    basket.Unit += request.Unit;
                    db.SaveChanges();
                    return;
                }

                db.Baskets.Add(request);
                db.SaveChanges();
            }
        }

        public void SessionSetLoginUser(string sessionId, int userId)
        {
            var db = _dbFactory();
            var baskets = db.Baskets.Where(a => a.SessionId == sessionId).ToList();
            if (baskets.Any())
            {
                baskets.ForEach(a =>
                {
                    a.UserId = userId;
                    a.SessionId = string.Empty;
                });
                db.SaveChanges();
            }
        }

        public void Edit(int id, Basket basketRequest)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == id);
            basket.IpAddress = basketRequest.IpAddress;
            basket.Price = basketRequest.Price;
            basket.ProductName = basketRequest.ProductName;
            basket.ProductUrl = basketRequest.ProductUrl;
            basket.SessionId = basketRequest.SessionId;
            basket.DateTime = basketRequest.DateTime;
            basket.OtherDetail = basketRequest.OtherDetail;
            basket.ProductId = basketRequest.ProductId;
            basket.Unit = basketRequest.Unit;
            basket.UserId = basketRequest.UserId;
            db.SaveChanges();
        }

        public void EditUnitChange(int userId, int basketId, int unit)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId && a.UserId == userId);

            if (unit < 1)
                unit = 1;

            if (basket == null)
                return;

            basket.Unit = unit;
            db.SaveChanges();
        }

        public void EditUnitChange(string sessionId, int basketId, int unit)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId && a.SessionId == sessionId);

            if (unit < 1)
                unit = 1;

            if (basket == null)
                return;

            basket.Unit = unit;
            db.SaveChanges();
        }

        public void EditUnitChange(int basketId, int unit)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId);

            if (unit < 1)
                unit = 1;

            basket.Unit = unit;
            db.SaveChanges();
        }

        public void Delete(int userId, int basketId)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId && a.UserId == userId);

            if (basket == null)
                return;

            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public void UserDeleteBaskets(int userId)
        {
            var db = _dbFactory();
            var baskets = db.Baskets.Where(a => a.UserId == userId);

            db.Baskets.RemoveRange(baskets);
            db.SaveChanges();
        }

        public void Delete(string sessionId, int basketId)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId && a.SessionId == sessionId);

            if (basket == null)
                return;

            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public void Delete(int basketId)
        {
            var db = _dbFactory();
            var basket = db.Baskets.FirstOrDefault(a => a.Id == basketId);

            if (basket == null)
                return;

            db.Baskets.Remove(basket);
            db.SaveChanges();
        }

        public BasketListDto GetTopBasketList(string sessionId, int userId)
        {
            var list = GetUserBasketList(sessionId, userId);
            var products = list.ProductList.Select(a => new BasketItemDto
            {
                BasketId = a.BasketId,
                Code = a.Code,
                DiscountAmount = a.DiscountAmount,
                OrginalTotalAmount = a.OrginalTotalAmount,
                ProductId = a.ProductId,
                ProductName = a.ProductName,
                Url = a.ProductUrl,
                Price = a.ProductPrice,
                SubTotalAmount = a.SubTotalAmount,
                TotalAmount = a.TotalAmount,
                Unit = a.Unit,
                BigPicture = a.BigPicture,
                MinPicture = a.MinPicture,
                DiscountPrice = a.ProductDiscountPrice,
                //TotalDiscountedAmount = a.TotalDiscountedAmount
            }).ToList();

            var result = new BasketListDto
            {
                BasketCampigns = list.BasketCampigns,
                BasketTotalSum = list.BasketTotalSum,
                ProductList = products
            };

            return result;
        }

        public BasketProductList GetUserBasketList(string sessionId, int userId)
        {
            var db = _dbFactory();
            //sepet listesi
            var list = (from basket in db.Baskets
                    join product in db.Products on basket.ProductId equals product.Id
                    join picture in db.Pictures on basket.ProductId equals picture.ProductId
                    where picture.IsShowcase
                          && product.Status == StatusTypes.Active
                    select new
                    {
                        MiniPic = picture.MinPicture,
                        BigPic = picture.BigPicture,
                        ProductUrl = product.Url,
                        BasketId = basket.Id,
                        basket.DateTime,
                        product.KdvOdd,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        basket.Unit,
                        product.Code,
                        product.DiscountOdd,
                        product.IsKdv,
                        basket.Price,
                        product.ProductType,
                        product.StockType,
                        basket.OtherDetail,
                        basket.UserId,
                        basket.SessionId
                    });

            //(basket.SessionId == sessionId || basket.UserId == userId)
            if (userId != 0)
                list = list.Where(a => a.UserId == userId);
            else
                list = list.Where(a => a.SessionId == sessionId);

            var items = list.ToList().Select(a =>
                {
                    var productPrice = !a.IsKdv
                        ? a.Price
                        : Function.KdvAmountProductDown(a.Price, a.KdvOdd);

                    var productDiscountPrice = productPrice - (a.DiscountOdd != 0
                        ? Function.ProductDiscount(productPrice, a.DiscountOdd)
                        : 0);

                    var kdvAmount = Function.KdvAmount(productDiscountPrice, a.KdvOdd, a.Unit);

                    var subTotalAmont = Function.SubTotalAmount(productDiscountPrice, a.Unit);

                    var totalDiscountAmount = a.DiscountOdd != 0
                        ? Function.ProductDiscountAmount(productPrice, a.DiscountOdd, a.Unit)
                        : 0;

                    var totalAmount = !a.IsKdv ? Function.TotalAmount(productDiscountPrice, a.KdvOdd, a.Unit) : Function.TotalAmount(productDiscountPrice, 0, a.Unit);

                    //var totalDiscountedAmount = totalAmount - totalDiscountAmount;

                    var item = new ProductDto
                    {
                        MinPicture = a.MiniPic,
                        BigPicture = a.BigPic,
                        BasketId = a.BasketId,
                        DateTime = a.DateTime,
                        KdvOdd = a.KdvOdd,
                        ProductId = a.ProductId,
                        ProductName = a.ProductName,
                        ProductUrl = a.ProductUrl,
                        ProductPrice = a.Price,
                        Unit = a.Unit,
                        Code = a.Code,
                        DiscountOdd = a.DiscountOdd,
                        IsKdv = a.IsKdv,

                        KdvAmount = kdvAmount,
                        TotalAmount = a.IsKdv ? totalAmount + kdvAmount : totalAmount,
                        SubTotalAmount = subTotalAmont,
                        DiscountAmount = totalDiscountAmount,
                        //TotalDiscountedAmount = totalDiscountedAmount,
                        ProductDiscountPrice = productDiscountPrice,

                        ProductType = Utility.AllProductConvert[a.ProductType],
                        StockType = Utility.AllStockConvert[a.StockType],
                        Detail = _orderService.GetOrderDetail(a.OtherDetail)
                    };

                    return item;
                }).ToList();

            //kargo firmasi secmeli yapilacak.
            var cargo = _cargoService.GetDefaultCargo();

            //urune gore indirimleri denetler
            //list = Function.ProductDiscountCalculate(list);

            //diptoplam alir
            var totalSum = Function.TotalAmountCalculate(items, cargo);

            //sepet kampanyalarina bakar
            var totalCampaign = _campaignService.CampaignCheck(cargo, totalSum);

            //sonuc listesi
            var result = new BasketProductList
            {
                ProductList = items,
                BasketTotalSum = totalSum,
                BasketCampigns = totalCampaign
            };

            return result;
        }

        public BasketProductList GetUserBasketList(int userId)
        {
            return GetUserBasketList(string.Empty, userId);

            //var db = _dbFactory();
            ////sepet listesi
            //var list = (from basket in db.Baskets
            //            join product in db.Products on basket.ProductId equals product.Id
            //            where basket.UserId == userId
            //                  && product.Status == StatusTypes.Active
            //            select new
            //            {
            //                ProductUrl = product.Url,
            //                BasketId = basket.Id,
            //                basket.DateTime,
            //                product.KdvOdd,
            //                ProductId = product.Id,
            //                ProductName = product.Name,
            //                ProductPrice = basket.Price,
            //                basket.Unit,
            //                product.Code,
            //                product.DiscountOdd,
            //                product.IsKdv,
            //                basket.Price,
            //                product.ProductType,
            //                product.StockType,
            //                basket.OtherDetail,
            //                basket.UserId
            //            }).ToList()
            //            .Select(a => new ProductDto
            //            {
            //                BasketId = a.BasketId,
            //                DateTime = a.DateTime,
            //                KdvOdd = a.KdvOdd,
            //                ProductId = a.ProductId,
            //                ProductName = a.ProductName,
            //                ProductPrice = a.ProductPrice,
            //                Unit = a.Unit,
            //                Code = a.Code,
            //                DiscountOdd = a.DiscountOdd,
            //                IsKdv = a.IsKdv,

            //                KdvAmount = !a.IsKdv ? Function.KdvAmount(a.Price, a.KdvOdd, a.Unit) : Function.KdvAmountProductDown(a.Price, a.KdvOdd),
            //                TotalAmount = Function.TotalAmount(a.Price, a.KdvOdd, a.Unit),
            //                SubTotalAmount = Function.SubTotalAmount(a.Price, a.Unit),
            //                DiscountAmount = a.DiscountOdd != 0 ? Function.ProductDiscountAmount(a.Price, a.DiscountOdd, a.Unit) : 0,

            //                ProductType = Utility.AllProductConvert[a.ProductType],
            //                StockType = Utility.AllStockConvert[a.StockType],
            //                Detail = _orderService.GetOrderDetail(a.OtherDetail)
            //            }).ToList();

            ////kargo firmasi secmeli yapilacak.
            //var cargo = _cargoService.GetDefaultCargo();

            ////urune gore indirimleri denetler
            //list = Function.ProductDiscountCalculate(list);

            ////diptoplam alir
            //var totalSum = Function.TotalAmountCalculate(list, cargo);

            ////sepet kampanyalarina bakar
            //var totalCampaign = _campaignService.CampaignCheck(cargo, totalSum);

            ////sonuc listesi
            //var result = new BasketProductList
            //{
            //    ProductList = list,
            //    BasketTotalSum = totalSum,
            //    BasketCampigns = totalCampaign
            //};

            //return result;
        }

        public List<ProductDto> GetUserBasketList(BasketSearchDto request)
        {
            var db = _dbFactory();
            //sepet listesi
            var list = (from basket in db.Baskets
                        join product in db.Products on basket.ProductId equals product.Id
                        join user in db.Users on basket.UserId equals user.Id into us
                        from user in us.DefaultIfEmpty()
                        where product.Status == StatusTypes.Active
                        select new BasketProduct
                        {
                            BasketId = basket.Id,
                            DateTime = basket.DateTime,
                            KdvOdd = product.KdvOdd,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            UserName = user == null ? "Guest" : user.Name,
                            UserSurname = user.Surname,
                            ProductPrice = product.Price,
                            Unit = basket.Unit,
                            Code = product.Code,
                            DiscountOdd = product.DiscountOdd,
                            IsKdv = product.IsKdv,

                            ProductType = product.ProductType,
                            StockType = product.StockType,
                            Detail = basket.OtherDetail
                        });

            if (!string.IsNullOrEmpty(request.Name))
                list = list.Where(a => a.UserName == request.Name);

            if (!string.IsNullOrEmpty(request.Name))
                list = list.Where(a => a.UserSurname == request.Surname);

            var defaultDate = new DateTime(1990, 1, 1);
            if (request.StartDate > defaultDate)
                list = list.Where(a => a.DateTime >= request.StartDate);

            if (request.EndDate > defaultDate)
            {
                var endDate = request.EndDate.AddDays(1).AddSeconds(-1);
                list = list.Where(a => a.DateTime <= endDate);
            }

            var baskets = list.ToList().Select(a =>
            {
                var productPrice = !a.IsKdv
                    ? a.ProductPrice
                    : Function.KdvAmountProductDown(a.ProductPrice, a.KdvOdd);

                var productDiscountPrice = productPrice - (a.DiscountOdd != 0
                    ? Function.ProductDiscount(productPrice, a.DiscountOdd)
                    : 0);

                var kdvAmount = Function.KdvAmount(productDiscountPrice, a.KdvOdd, a.Unit);

                var subTotalAmont = Function.SubTotalAmount(productDiscountPrice, a.Unit);

                var totalDiscountAmount = a.DiscountOdd != 0
                    ? Function.ProductDiscountAmount(productPrice, a.DiscountOdd, a.Unit)
                    : 0;

                var totalAmount = !a.IsKdv ? Function.TotalAmount(productDiscountPrice, a.KdvOdd, a.Unit) : Function.TotalAmount(productDiscountPrice, 0, a.Unit);

                //var totalDiscountedAmount = totalAmount - totalDiscountAmount;

                var item = new ProductDto
                {
                    BasketId = a.BasketId,
                    DateTime = a.DateTime,
                    KdvOdd = a.KdvOdd,
                    ProductId = a.ProductId,
                    ProductName = a.ProductName,
                    ProductPrice = a.ProductPrice,
                    Unit = a.Unit,
                    Code = a.Code,
                    DiscountOdd = a.DiscountOdd,
                    IsKdv = a.IsKdv,
                    UserName = a.UserName,
                    UserSurname = a.UserSurname,

                    KdvAmount = kdvAmount,
                    TotalAmount = a.IsKdv ? totalAmount + kdvAmount : totalAmount,
                    SubTotalAmount = subTotalAmont,
                    DiscountAmount = totalDiscountAmount,
                    //TotalDiscountedAmount = totalDiscountedAmount,
                    ProductDiscountPrice = productDiscountPrice,

                    ProductType = Utility.AllProductConvert[a.ProductType],
                    StockType = Utility.AllStockConvert[a.StockType],
                    Detail = _orderService.GetOrderDetail(a.Detail)
                };

                return item;
            }).ToList();

            return baskets;
        }

        public BasketProductList GetUserBasketList(int userId, DateTime startDate, DateTime endDate)
        {
            var db = _dbFactory();
            //sepet listesi
            var list = (from basket in db.Baskets
                        join product in db.Products on basket.ProductId equals product.Id
                        where product.Status == AdminProject.Models.StatusTypes.Active
                        select new ProductDto
                        {
                            BasketId = basket.Id,
                            DateTime = basket.DateTime,
                            KdvOdd = product.KdvOdd,
                            ProductId = product.Id,
                            ProductName = product.Name,
                            ProductPrice = product.Price,
                            Unit = basket.Unit,
                            Code = product.Code,
                            IsKdv = product.IsKdv,

                            DiscountOdd = product.DiscountOdd,
                            DiscountAmount = product.DiscountOdd != 0 ? Function.ProductDiscountAmount(product.Price, product.DiscountOdd, basket.Unit) : 0,
                            SubTotalAmount = Function.SubTotalAmount(product.Price, basket.Unit),
                            KdvAmount = !product.IsKdv ? Function.KdvAmount(product.Price, product.KdvOdd, basket.Unit) : Function.KdvAmountProductDown(product.Price, product.KdvOdd),
                            TotalAmount = Function.TotalAmount(product.Price, product.KdvOdd, basket.Unit),

                            ProductType = Utility.AllProductConvert[product.ProductType],
                            StockType = Utility.AllStockConvert[product.StockType],
                            Detail = JsonConvert.DeserializeObject<List<ProductDetail>>(basket.OtherDetail)
                        }).ToList();

            //kargo firmasi secmeli yapilacak.
            var cargo = _cargoService.GetDefaultCargo();

            //urune gore indirimleri denetler
            list = Function.ProductDiscountCalculate(list);

            //diptoplam alir
            var totalSum = Function.TotalAmountCalculate(list, cargo);

            //sepet kampanyalarina bakar
            var totalCampaign = _campaignService.CampaignCheck(cargo, totalSum);

            //sonuc listesi
            var result = new BasketProductList
            {
                ProductList = list,
                BasketTotalSum = totalSum,
                BasketCampigns = totalCampaign
            };

            return result;
        }
    }
}