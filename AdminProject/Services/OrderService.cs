using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using AdminProject.Helpers;
using AdminProject.Infrastructure;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using Newtonsoft.Json;

namespace AdminProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly IInvoiceService _invoiceService;
        private readonly ICargoService _cargoService;
        private readonly IAddressService _addressService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ICampaignService _campaignService;

        public OrderService(Func<AdminDbContext> dbFactory, IInvoiceService invoiceService, ICargoService cargoService, IAddressService addressService, IUserService userService, IProductService productService, ICampaignService campaignService)
        {
            _dbFactory = dbFactory;
            _invoiceService = invoiceService;
            _cargoService = cargoService;
            _addressService = addressService;
            _userService = userService;
            _productService = productService;
            _campaignService = campaignService;
        }

        public void Add(Order order)
        {
            var db = _dbFactory();
            db.Orders.Add(order);
            db.SaveChanges();
        }

        public void Edit(int id, Order orderRequest)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.Id == id);
            if (order == null)
                throw new CustomException("Satış bulunamadı. Id: " + id);

            AddOrderTemplate(order, "Order Edited.");

            order.AddressId = orderRequest.AddressId;
            order.CargoId = orderRequest.CargoId;
            order.CargoNr = orderRequest.CargoNr;
            order.CauseOfRefund = orderRequest.CauseOfRefund;
            order.CreateDate = orderRequest.CreateDate;
            order.Description = orderRequest.Description;
            order.InvoiceId = orderRequest.InvoiceId;
            order.OrderNote = orderRequest.OrderNote;
            order.OrderNr = orderRequest.OrderNr;
            order.OrderType = orderRequest.OrderType;
            order.ParentOrderId = orderRequest.ParentOrderId;
            order.PayType = orderRequest.PayType;
            order.UpdateDate = DateTime.Now;
            order.UserId = orderRequest.UserId;
            order.UpdateUserId = Utility.SessionCheck().Id;
            db.SaveChanges();
        }

        public void AddTransmitterOrder(Order order, List<OrderProductAssg> products)
        {
            var db = _dbFactory();

            db.Orders.Add(order);
            db.SaveChanges();

            var orderId = order.Id;
            products.ForEach(a => a.OrderId = orderId);
            db.OrderProductAssgs.AddRange(products);
            db.SaveChanges();
        }

        public Order GetOrder(int id)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.Id == id);
            return order;
        }

        public List<ProductDetail> GetOrderDetail(string detail)
        {
            if (string.IsNullOrEmpty(detail))
                return new List<ProductDetail>();

            return Tool.ProductDetailDeserialization(detail);
        }

        public List<OrderProductAssg> GetOrderProductIds(int orderId)
        {
            var db = _dbFactory();
            var productIds = db.OrderProductAssgs.Where(a => a.OrderId == orderId).ToList();

            return productIds;
        }

        public List<ProductDto> GetOrderProduct(int id)
        {
            var orderProducts = GetOrderProductIds(id);
            var orderProductIds = orderProducts.Select(a => a.ProductId).ToArray();
            var products = _productService.GetProduct(orderProductIds);

            var productsDto = products.Select(pro =>
            {
                var orderProd = orderProducts.FirstOrDefault(a => a.ProductId == pro.Id);

                var product = new ProductDto
                {
                    BasketId = orderProd.Id,
                    Code = pro.Code,
                    ProductName = pro.Name,
                    DateTime = pro.CreateDate,
                    Unit = orderProd.Unit,
                    IsKdv = orderProd.IsKdv,
                    KdvOdd = orderProd.KdvOdd,
                    ProductPrice = orderProd.Price,
                    KdvAmount = orderProd.KdvAmount,
                    ProductId = orderProd.ProductId,
                    DiscountOdd = orderProd.DiscountOdd,
                    TotalAmount = orderProd.TotalAmount,
                    CauseOfRefund = orderProd.CauseOfRefund,
                    PurchasePrice = orderProd.PurchasePrice,
                    SubTotalAmount = orderProd.SubTotalAmount,
                    DiscountAmount = orderProd.DiscountAmount,
                    OrginalTotalAmount = orderProd.OrginalTotalAmount,
                    Detail = GetOrderDetail(orderProd.OtherDetail), //json detail
                    StockType = Utility.AllStockConvert[pro.StockType],
                    ProductType = Utility.AllProductConvert[pro.ProductType],
                    OrderType = orderProd.OrderType
                };

                return product;
            }).ToList();

            return productsDto;
        }

        public OrderResult GetOrderDetailResult(int id)
        {
            var order = GetOrder(id);
            var invoice = _invoiceService.GetInvoice(order.InvoiceId);
            var cargo = _cargoService.GetCargo(order.CargoId);
            var address = _addressService.GetAddress(order.AddressId);
            var user = _userService.GetUser(order.UserId);

            var productsDto = GetOrderProduct(id);

            //kargo firmasi secmeli yapilacak.
            var totalSum = Function.TotalAmountCalculate(productsDto, cargo);
            var totalCampaign = _campaignService.CampaignCheck(cargo, totalSum);

            var result = new OrderResult
            {
                AddressDetail = address,
                CargoDetail = cargo,
                InvoiceDetail = invoice,
                OrderDetail = order,
                UserDetail = user,
                Products = productsDto,
                TotalSum = totalSum,
                CampaignSumCalculate = totalCampaign
            };

            return result;
        }

        public void ChangeOrderStatus(int orderId, OrderTypes orderType, string causeOfRefund)
        {
            var db = _dbFactory();

            var order = db.Orders.FirstOrDefault(a => a.Id == orderId);
            if (order == null)
                return;

            AddOrderTemplate(order, "Status Changed");

            order.CauseOfRefund = causeOfRefund;
            order.OrderType = orderType;
            order.UpdateUserId = Utility.SessionCheck().Id;
            db.SaveChanges();

            ChangeOrderProductAllOrderStatus(orderId, orderType);
        }

        public void ChangeOrderProductAllOrderStatus(int orderId, OrderTypes orderType)
        {
            var db = _dbFactory();

            var products = db.OrderProductAssgs.Where(a => a.OrderId == orderId).ToList();
            products.ForEach(item => item.OrderType = orderType);
            db.SaveChanges();
        }

        public void ChangeOrderProductSingleOrderStatus(int id, OrderTypes orderType, string note)
        {
            var db = _dbFactory();

            var products = db.OrderProductAssgs.FirstOrDefault(a => a.Id == id);
            products.OrderType = orderType;
            products.CauseOfRefund = note;
            db.SaveChanges();
        }

        public void DeleteOrderProductRow(int id)
        {
            var db = _dbFactory();

            var product = db.OrderProductAssgs.FirstOrDefault(a => a.Id == id);
            db.OrderProductAssgs.Remove(product);
            db.SaveChanges();
        }

        private void AddOrderTemplate(Order order, string changeDescription)
        {
            var db = _dbFactory();

            var orderTemp = new OrderTemp
            {
                AddressId = order.AddressId,
                CargoId = order.CargoId,
                CargoNr = order.CargoNr,
                CauseOfRefund = order.CauseOfRefund,
                CreateDate = order.CreateDate,
                Description = order.Description,
                InvoiceId = order.InvoiceId,
                OrderId = order.Id,
                OrderNote = order.OrderNote,
                OrderNr = order.OrderNr,
                OrderType = order.OrderType,
                OtherDetail = order.OtherDetail,
                PayType = order.PayType,
                UpdateDate = order.UpdateDate,
                UpdateUserId = order.UpdateUserId,
                UserId = order.UserId,
                ChangeDescription = changeDescription,
                CreateUserId = order.CreateUserId,
                DiscountAmount = order.DiscountAmount,
                IpAddress = order.IpAddress,
                IsCampaignApplied = order.IsCampaignApplied,
                KdvAmount = order.KdvAmount,
                TotalAmount = order.TotalAmount
            };

            db.OrderTemps.Add(orderTemp);
            db.SaveChanges();
        }

        public List<OrderResult> GetOrderDetailResult(int[] id)
        {
            var result = id.Select(GetOrderDetailResult).ToList();
            return result;
        }

        public List<Order> GetUserOrders(int userId)
        {
            var db = _dbFactory();

            var orders = db.Orders.Where(a => a.UserId == userId).OrderByDescending(a => a.CreateDate).ToList();
            return orders;
        }

        public PagerList<OrderSearchResultDto> GetOrderSearch(OrderSearchRequestDto request)
        {
            var db = _dbFactory();

            var orders = from order in db.Orders
                join user in db.Users
                on order.UserId equals user.Id
                select new OrderSearchResultDto
                {
                    City = user.City,
                    Country = user.Country,
                    CreateDate = order.CreateDate,
                    Name = user.Name,
                    OrderId = order.Id,
                    OrderNote = order.OrderNote,
                    Region = user.Region,
                    Surname = user.Surname,
                    UserId = user.Id,
                    Email = user.Email,
                    AddressId = order.AddressId,
                    CargoId = order.CargoId,
                    CargoNr = order.CargoNr,
                    InvoiceId = order.InvoiceId,
                    OrderNr = order.OrderNr,
                    //ShipmentType = order.ShipmentType,
                    OrderType = order.OrderType,
                    PayType = order.PayType
                };

            if (!string.IsNullOrEmpty(request.OrderNr))
            {
                var orderNr = request.OrderNr;
                orders = orders.Where(a => a.OrderNr == orderNr);
                var orderResult = new PagerList<OrderSearchResultDto>
                {
                    List = orders.ToList(),
                    TotalCount = orders.Count()
                };

                return orderResult;
            }

            var defaultDate = new DateTime(1970, 1, 1);

            if (!string.IsNullOrEmpty(request.Name))
                orders = orders.Where(a => a.Name == request.Name);

            if (!string.IsNullOrEmpty(request.Surname))
                orders = orders.Where(a => a.Surname == request.Surname);

            if (!string.IsNullOrEmpty(request.Email))
                orders = orders.Where(a => a.Email == request.Email);

            if (request.StartDate >= defaultDate)
                orders = orders.Where(a => a.CreateDate >= request.StartDate);

            if (request.EndDate <= defaultDate)
                orders = orders.Where(a => a.CreateDate <= request.EndDate);

            if (request.OrderType != OrderTypes.All)
                orders = orders.Where(a => a.OrderType == request.OrderType);

            if (request.PayType != PayTypes.All)
                orders = orders.Where(a => a.PayType == request.PayType);

            //if (request.ShipmentType != ShipmentTypes.All)
            //    orders = orders.Where(a => a.ShipmentType == request.ShipmentType);

            var result = new PagerList<OrderSearchResultDto>
            {
                List = orders.ToList(),
                TotalCount = orders.Count()
            };

            return result;
        }

        public PagerList<OrderSearchResultDto> GetOrderHistory(int orderId)
        {
            var db = _dbFactory();

            var orders = from order in db.OrderTemps
                         join user in db.Users
                         on order.UserId equals user.Id
                         where order.OrderId == orderId
                         select new OrderSearchResultDto
                         {
                             City = user.City,
                             Country = user.Country,
                             CreateDate = order.CreateDate,
                             UpdateDate = order.UpdateDate,
                             Name = user.Name,
                             OrderId = order.Id,
                             OrderNote = order.OrderNote,
                             Region = user.Region,
                             Surname = user.Surname,
                             UserId = user.Id,
                             Email = user.Email,
                             AddressId = order.AddressId,
                             CargoId = order.CargoId,
                             CargoNr = order.CargoNr,
                             InvoiceId = order.InvoiceId,
                             OrderNr = order.OrderNr,
                             //ShipmentType = order.ShipmentType,
                             OrderType = order.OrderType,
                             PayType = order.PayType,
                             UpdateUserId = order.UpdateUserId
                         };
            var result = new PagerList<OrderSearchResultDto>
            {
                List = orders.ToList(),
                TotalCount = orders.Count()
            };

            return result;
        }

        public void ChangeOrderStatus(int id, int userId, OrderTypes orderType)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.Id == id && a.UserId == userId);

            AddOrderTemplate(order, "Change order status");

            order.OrderType = orderType;
            order.UpdateDate = DateTime.Now;
            order.UpdateUserId = userId;
            db.SaveChanges();
        }

        public void ChangeOrderStatus(int id, OrderTypes orderType)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.Id == id);

            AddOrderTemplate(order, "Change order status.");

            order.OrderType = orderType;
            order.UpdateDate = DateTime.Now;
            order.UpdateUserId = Utility.SessionCheck().Id;
            db.SaveChanges();
        }

        public void ChangeOrderCargoNumber(int id, string cargoNr)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.Id == id);

            AddOrderTemplate(order, "Change cargo nr.");

            order.CargoNr = cargoNr;
            order.UpdateDate = DateTime.Now;
            order.UpdateUserId = Utility.SessionCheck().Id;
            db.SaveChanges();
        }

        public List<ProductDto> GetUserOrderDetail(int userId, int orderId)
        {
            var db = _dbFactory();
            var order = db.Orders.FirstOrDefault(a => a.UserId == userId && a.Id == orderId);
            if (order == null)
                return new List<ProductDto>();

            var orderProducts = GetOrderProductIds(orderId);
            var orderProductIds = orderProducts.Select(a => a.ProductId).ToArray();

            var products = _productService.GetProduct(orderProductIds);
            var result = products.Select(pro =>
            {
                var orderProd = orderProducts.FirstOrDefault(a => a.ProductId == pro.Id);

                var product = new ProductDto
                {
                    BasketId = orderProd.Id,
                    Code = pro.Code,
                    ProductName = pro.Name,
                    DateTime = pro.CreateDate,
                    Unit = orderProd.Unit,
                    IsKdv = orderProd.IsKdv,
                    KdvOdd = orderProd.KdvOdd,
                    ProductPrice = orderProd.Price,
                    KdvAmount = orderProd.KdvAmount,
                    ProductId = orderProd.ProductId,
                    DiscountOdd = orderProd.DiscountOdd,
                    TotalAmount = orderProd.TotalAmount,
                    CauseOfRefund = orderProd.CauseOfRefund,
                    PurchasePrice = orderProd.PurchasePrice,
                    SubTotalAmount = orderProd.SubTotalAmount,
                    DiscountAmount = orderProd.DiscountAmount,
                    OrginalTotalAmount = orderProd.OrginalTotalAmount,
                    Detail = GetOrderDetail(orderProd.OtherDetail), //json detail
                    StockType = Utility.AllStockConvert[pro.StockType],
                    ProductType = Utility.AllProductConvert[pro.ProductType],
                    OrderType = orderProd.OrderType
                };

                return product;
            }).ToList();

            return result;
        }

        public string GetNewOrderNumber()
        {
            var number = $"PYDR-{DateTime.Now:yyyyMMddHHmmss}";
            return number;
        }

        public SelectList AllOrderTypeList(string selectedValue)
        {
            var list = Utility.AllOrderConvert.Select(a => new ListItem
            {
                Text = a.Value,
                Value = a.Key.ToString()
            }).OrderBy(a => a.Text).ToList();

            var selectList = new SelectList(list, "Value", "Text", selectedValue);
            return selectList;
        }

        public SelectList AllPayTypeList(string selectedValue)
        {
            var list = Utility.AllPayConvert.Select(a => new ListItem
            {
                Text = a.Value,
                Value = a.Key.ToString()
            }).OrderBy(a => a.Text).ToList();

            var selectList = new SelectList(list, "Value", "Text", selectedValue);
            return selectList;
        }
    }
}