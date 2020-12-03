using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;

namespace AdminProject.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;

        public OrderController(RuntimeSettings setting, IOrderService orderService) : base(setting)
        {
            _orderService = orderService;
        }

        public ActionResult List(string Name, string Surname, string Email, string StartDate, string EndDate, string OrderNr, int Take = 20, int Skip = 1, PayTypes PayType = PayTypes.All, OrderTypes OrderType = OrderTypes.All)
        {
            SetPageHeader("Order", "Search List");

            ViewBag.OrderTypeList = _orderService.AllOrderTypeList(OrderType.ToString());
            ViewBag.PayTypeList = _orderService.AllPayTypeList(PayType.ToString());

            var request = new OrderSearchRequestDto
            {
                Email = Email,
                Name = Name,
                OrderNr = OrderNr,
                OrderType = OrderType,
                PayType = PayType,
                Surname = Surname
            };

            ViewBag.Orders = new PagerList<OrderSearchResultDto>();
            var sDate = DateTime.Now.AddDays(-31);
            var eDate = DateTime.Now.AddDays(1);

            if(!string.IsNullOrEmpty(StartDate))
            if (!Utility.DateTimeParsing(StartDate, out sDate))
                ModelState.AddModelError("StartDate", "Start date is format error.");

            if (!string.IsNullOrEmpty(EndDate))
                if (!Utility.DateTimeParsing(EndDate, out eDate))
                ModelState.AddModelError("EndDate", "End date is format error.");

            if (!ModelState.IsValid)
                return View(request);

            request.StartDate = sDate;
            request.EndDate = eDate;

            ViewBag.Orders = _orderService.GetOrderSearch(request);

            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "Skip").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            ViewBag.UrlAddress = string.Format("/Admin/Order/List?{0}", nameValue.ToQueryString());

            return View(request);
        }

        public ActionResult View(int id)
        {
            SetPageHeader("Order", "Detail View");

            var result = _orderService.GetOrderDetailResult(id);

            if (result.OrderDetail == null)
                return RedirectToAction("List");

            //ViewBag.ShipmentTypeList = _orderService.AllShipmentTypeList(result.OrderDetail.ShipmentType.ToString());
            ViewBag.PayTypeList = _orderService.AllPayTypeList(result.OrderDetail.PayType.ToString());
            ViewBag.OrderTypeList = _orderService.AllOrderTypeList(result.OrderDetail.OrderType.ToString());

            return View(result);
        }

        public ActionResult History(int id)
        {
            SetPageHeader("Order", "History");

            var result = _orderService.GetOrderHistory(id);
            ViewBag.Orders = result;
            return View();
        }

        public ActionResult QuickViewProduct(int id)
        {
            var result = _orderService.GetOrderProduct(id);
            ViewBag.Products = result;
            return View();
        }

        public ActionResult ChangeCargo(int id)
        {
            var order = _orderService.GetOrder(id);
            ViewBag.OrderNr = order.OrderNr;
            ViewBag.CargoNr = order.CargoNr;
            ViewBag.Id = id;

            return View();
        }

        [HttpPost]
        public ActionResult ChangeCargo(int id, string cargoNr)
        {
            _orderService.ChangeOrderCargoNumber(id, cargoNr);
            Updated();
            return RedirectToAction("List");
        }

        public ActionResult ChangeStatus(int id, string actionType)
        {
            if (actionType == "success")
                _orderService.ChangeOrderStatus(id, OrderTypes.Complete);

            if (actionType == "failed")
                _orderService.ChangeOrderStatus(id, OrderTypes.Canceled);

            Updated();

            return RedirectToAction("List");
        }

        [HttpPost]
        public ActionResult View(int id, OrderTypes OrderType, string CauseOfRefund)
        {
            _orderService.ChangeOrderStatus(id, OrderType, CauseOfRefund);

            Updated();

            return RedirectToAction("View", id);
        }

        [HttpPost]
        public JsonResult ChangeProductOrderType(int id, OrderTypes orderType, string note)
        {
            _orderService.ChangeOrderProductSingleOrderStatus(id, orderType, note);

            return Json("Ok", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteOrderProductRow(int id, int orderId)
        {
            _orderService.DeleteOrderProductRow(id);

            Deleted();

            return RedirectToAction("View", new { id = orderId });
        }
    }
}