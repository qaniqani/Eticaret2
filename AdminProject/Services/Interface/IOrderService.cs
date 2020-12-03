using System.Collections.Generic;
using System.Web.Mvc;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Models;

namespace AdminProject.Services.Interface
{
    public interface IOrderService
    {
        void Add(Order order);
        void Edit(int id, Order orderRequest);
        Order GetOrder(int id);
        List<OrderProductAssg> GetOrderProductIds(int orderId);
        OrderResult GetOrderDetailResult(int id);
        List<OrderResult> GetOrderDetailResult(int[] id);
        List<Order> GetUserOrders(int userId);
        void ChangeOrderStatus(int id, int userId, OrderTypes orderType);
        void ChangeOrderStatus(int id, OrderTypes orderType);
        void ChangeOrderCargoNumber(int id, string cargoNr);
        string GetNewOrderNumber();
        List<ProductDto> GetOrderProduct(int id);
        SelectList AllPayTypeList(string selectedValue);
        SelectList AllOrderTypeList(string selectedValue);
        PagerList<OrderSearchResultDto> GetOrderHistory(int orderId);
        PagerList<OrderSearchResultDto> GetOrderSearch(OrderSearchRequestDto request);
        void ChangeOrderStatus(int orderId, OrderTypes orderType, string causeOfRefund);
        List<ProductDetail> GetOrderDetail(string detail);
        void ChangeOrderProductSingleOrderStatus(int id, OrderTypes orderType, string note);
        void DeleteOrderProductRow(int id);
        void AddTransmitterOrder(Order order, List<OrderProductAssg> products);
        List<ProductDto> GetUserOrderDetail(int userId, int orderId);
    }
}