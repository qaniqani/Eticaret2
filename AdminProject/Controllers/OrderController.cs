using System;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Helpers;
using AdminProject.Attributes;
using AdminProject.Services.Interface;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Controllers
{
    [RoutePrefix("order")]
    public class OrderController : BaseController
    {
        private readonly IAddressService _addressService;
        private readonly IInvoiceService _invoiceService;
        private readonly IBasketService _basketService;
        private readonly IBankService _bankService;
        private readonly ICargoService _cargoService;
        private readonly IIyzipayService _iyzicoService;
        private readonly IOrderService _orderService;
        private readonly RuntimeSettings _settings;

        public OrderController(IAddressService addressService, IInvoiceService invoiceService,
            IBasketService basketService, IBankService bankService, ICargoService cargoService, 
            IOrderService orderService, RuntimeSettings settings, IIyzipayService iyzicoService)
        {
            _addressService = addressService;
            _invoiceService = invoiceService;
            _basketService = basketService;
            _bankService = bankService;
            _cargoService = cargoService;
            _orderService = orderService;
            _settings = settings;
            _iyzicoService = iyzicoService;
        }

        [Route("delivery-address")]
        [HttpGet]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult DeliveryAddress()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var userId = Utility.UserCheck().Id;
            var deliveryAddress = _addressService.GetUserAddressUserActiveList(userId);
            ViewBag.DeliveryAddress = deliveryAddress;

            ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
            ViewBag.InvoiceType = DropdownTypes.GetUserInvoiceTypes(InoviceTypes.Personal);

            var sessionId = GetSessionId();

            var list = _basketService.GetTopBasketList(sessionId, userId);
            ViewBag.Baskets = list;

            return View();
        }

        [Route("delivery-address")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult DeliveryAddress(string deliveryId, bool isInvoiceAndDelivery, string taxNr, string taxOffice,
            InoviceTypes invoiceType)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var userId = Utility.UserCheck().Id;
            var deliveryAddress = _addressService.GetUserAddressUserActiveList(userId);
            ViewBag.DeliveryAddress = deliveryAddress;

            ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
            ViewBag.InvoiceType = DropdownTypes.GetUserInvoiceTypes(InoviceTypes.Personal);

            var sessionId = GetSessionId();
            var list = _basketService.GetTopBasketList(sessionId, userId);
            ViewBag.Baskets = list;

            if (string.IsNullOrEmpty(deliveryId))
                ModelState.AddModelError("DeliveryId", "Adres seçmeniz zorunludur.");

            if (!ModelState.IsValid)
            {
                var errors = GetErrorMessage(ModelState.Values);
                ErrorMessage(errors);
                return View();
            }

            var delivery = _addressService.GetAddress(deliveryId.ToInt32());

            if (isInvoiceAndDelivery)
            {
                if (string.IsNullOrEmpty(taxNr))
                    ModelState.AddModelError("TaxNr", "Vergi numaranız zorunludur.");

                if (string.IsNullOrEmpty(taxOffice))
                    ModelState.AddModelError("TaxOffice", "Vergi dairesi zorunludur.");

                if (!ModelState.IsValid)
                {
                    var errors = GetErrorMessage(ModelState.Values);
                    ErrorMessage(errors);
                    return View();
                }

                var invoice = new Invoice
                {
                    Address = delivery.AddressDetail,
                    City = delivery.City,
                    Country = "TURKIYE",
                    Gsm = delivery.Gsm,
                    InvoiceSaveName = delivery.AddressSaveName,
                    InvoiceType = invoiceType,
                    IsEInvoice = false,
                    NameSurname = delivery.NameSurname,
                    Phone = delivery.Phone,
                    Region = delivery.Region,
                    Status = delivery.Status,
                    TaxNr = taxNr,
                    TaxOffice = taxOffice,
                    UserId = userId
                };

                var invoiceId = _invoiceService.AddReturnId(invoice);

                var url = $"/order/contract?invoice={invoiceId}&delivery={deliveryId}";

                return Redirect(url);
            }

            var invoiceUrl = $"/order/invoice-address?delivery={deliveryId}";
            return Redirect(invoiceUrl);
        }

        [Route("invoice-address")]
        [HttpGet]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult InvoiceAddress(string delivery)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            if (string.IsNullOrEmpty(delivery))
            {
                ErrorMessage("Lütfen önce teslimat adresini seçiniz.");
                return Redirect("/order/delivery-address");
            }

            var userId = Utility.UserCheck().Id;
            var result = _invoiceService.GetUserInoive(userId);

            ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
            ViewBag.DeliveryId = delivery;

            var sessionId = GetSessionId();

            var list = _basketService.GetTopBasketList(sessionId, userId);
            ViewBag.Baskets = list;

            return View(result);
        }

        [Route("invoice-address")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult InvoiceAddress(string delivery, string invoiceId)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            if (string.IsNullOrEmpty(delivery))
            {
                ErrorMessage("Lütfen önce teslimat adresini seçiniz.");
                return Redirect("/order/delivery-address");
            }

            if (string.IsNullOrEmpty(invoiceId))
            {
                ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
                ViewBag.DeliveryId = delivery;

                var userId = Utility.UserCheck().Id;
                var result = _invoiceService.GetUserInoive(userId);

                var sessionId = GetSessionId();
                var list = _basketService.GetTopBasketList(sessionId, userId);
                ViewBag.Baskets = list;

                ErrorMessage("Fatura adresi seçmeniz zorunludur");
                return View(result);
            }

            var url = $"/order/contract?delivery={delivery}&invoice={invoiceId}";

            return Redirect(url);
        }

        [Route("contract")]
        [HttpGet]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Contract(string delivery, string invoice)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            GetContractDetail(delivery, invoice);

            return View();
        }

        private void GetContractDetail(string delivery, string invoice)
        {
            var user = Utility.UserCheck();
            var userId = user.Id;
            var sessionId = GetSessionId();
            var selectedCargo = _cargoService.GetDefaultCargo();
            var deliveryAddress = _addressService.GetAddress(delivery.ToInt32());
            var invoiceAddress = _invoiceService.GetInvoice(invoice.ToInt32());
            var list = _basketService.GetTopBasketList(sessionId, userId);
            ViewBag.Baskets = list;

            //[ALICI]
            //[ADRESI]
            //[TELEFON]
            //[EMAIL]
            //[TABLO]
            //[TESLIMATADRESI]
            //[TESLIMEDILECEKKISI]
            //[FATURAADRESI]
            //[TARIH] dd.mm.yyyy

            var tableTemplate = @"
                                <table style='width:100%;'>
                                    <thead>
                                    <tr>
                                        <td style='font-weight:bold;'>Ürün Adı<td>
                                        <td style='font-weight:bold; text-align:center;'>Adet<td>
                                        <td style='font-weight:bold; text-align:center;'>Tutar<td>
                                    </tr>
                                    </thead>
                                    <tbody>
                                        {0}
                                    </tbody>
                                </table>";
            var tableRowTemplate = @"
                                    <tr>
                                        <td>{0}<td>
                                        <td style='text-align:center;'>{1}<td>
                                        <td style='text-align:center;'>{2} TL<td>
                                    </tr>{3}";

            var rows =
                list.ProductList.Select(
                    a => string.Format(tableRowTemplate, a.ProductName, a.Unit, a.TotalAmount.ToString("n2"), "<br>")).ToList();

            var rowsRender = string.Empty;
            rows.ForEach(a => { rowsRender += a; });

            var createTable = string.Format(tableTemplate, rowsRender);

            //[ALICI]
            //[ADRESI]
            //[TELEFON]
            //[EMAIL]
            //[TABLO]
            //[TESLIMATADRESI]
            //[TESLIMEDILECEKKISI]
            //[FATURAADRESI]
            //[TARIH] dd.mm.yyyy
            var distanceSales = _settings.DistanceSalesContract;
            distanceSales = distanceSales.Replace("[ALICI]", user.Name + " " + user.Surname);
            distanceSales = distanceSales.Replace("[ADRESI]", user.Address);
            distanceSales = distanceSales.Replace("[TELEFON]", user.Phone);
            distanceSales = distanceSales.Replace("[EMAIL]", user.Email);
            distanceSales = distanceSales.Replace("[TABLO]", createTable);
            distanceSales = distanceSales.Replace("[TESLIMATADRESI]", deliveryAddress.AddressDetail);
            distanceSales = distanceSales.Replace("[TESLIMEDILECEKKISI]", deliveryAddress.NameSurname);
            distanceSales = distanceSales.Replace("[FATURAADRESI]", invoiceAddress.Address);
            distanceSales = distanceSales.Replace("[TARIH]", DateTime.Now.ToString("dd.MM.yyyy"));

            //[ALICI]
            //[ADRESI]
            //[TELEFON]
            //[EMAIL]
            //[TABLO]
            //[KARGOUCRETI]
            //[TARIH] dd.mm.yyyy
            var preliminaryInformation = _settings.PreliminaryInformationForm;
            preliminaryInformation = preliminaryInformation.Replace("[ALICI]", user.Name + " " + user.Surname);
            preliminaryInformation = preliminaryInformation.Replace("[ADRESI]", user.Address);
            preliminaryInformation = preliminaryInformation.Replace("[TELEFON]", user.Phone);
            preliminaryInformation = preliminaryInformation.Replace("[EMAIL]", user.Email);
            preliminaryInformation = preliminaryInformation.Replace("[TABLO]", createTable);
            preliminaryInformation = preliminaryInformation.Replace("[KARGOUCRETI]", selectedCargo.Price.ToString("n2"));
            preliminaryInformation = preliminaryInformation.Replace("[TARIH]", DateTime.Now.ToString("dd.MM.yyyy"));

            //[KARGO]
            var rightWithdraw = _settings.RightToWithdraw;
            rightWithdraw = rightWithdraw.Replace("[KARGO]", selectedCargo.Name);

            ViewBag.Distance = distanceSales;
            ViewBag.Information = preliminaryInformation;
            ViewBag.Withdraw = rightWithdraw;

            var url = $"/order/contract?delivery={delivery}&invoice={invoice}";
            ViewBag.Url = url;

            TotalPrice(list);
        }

        [Route("contract")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Contract(string delivery, string invoice, string orderNote)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var basketInfo = new PreSaveOrderInfo
            {
                DeliveryId = delivery,
                InvoiceId = invoice,
                OrderNote = orderNote
            };

            BasketInfo(basketInfo);

            var paymentAddress = $"/order/payment?delivery={delivery}&invoice={invoice}";
            return Redirect(paymentAddress);
        }

        [Route("payment")]
        [HttpGet]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Payment(string delivery, string invoice)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var user = Utility.UserCheck();
            var userId = user.Id;
            var sessionId = GetSessionId();
            var list = _basketService.GetTopBasketList(sessionId, userId);
            var deliveryAddress = _addressService.GetAddress(delivery.ToInt32());
            var invoiceAddress = _invoiceService.GetInvoice(invoice.ToInt32());
            ViewBag.Baskets = list;

            if (string.IsNullOrEmpty(delivery))
            {
                ErrorMessage("Lütfen önce teslimat adresini seçiniz.");
                return Redirect("/order/delivery-address");
            }

            if (!string.IsNullOrEmpty(delivery) && string.IsNullOrEmpty(invoice))
            {
                ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
                ViewBag.DeliveryId = delivery;

                var result = _invoiceService.GetUserInoive(userId);

                ErrorMessage("Fatura adresi seçmeniz zorunludur");
                return View("InvoiceAddress", result);
            }

            var banks = _bankService.GetActiveBankList();
            ViewBag.Banks = banks;

            var payDoors = _cargoService.ActiveDoorPayList();
            ViewBag.PayDoors = payDoors;

            ViewBag.Delivery = delivery;
            ViewBag.Invoice = invoice;

            var orderNumber = _orderService.GetNewOrderNumber();
            TempData["OrderNumber"] = orderNumber;
            OrderNumber(orderNumber);

            var basketInfo = BasketInfo();
            basketInfo.OrderNr = orderNumber;
            BasketInfo(basketInfo);

            //Kredi karti entegrasyonu...
            var checkOutForm = _iyzicoService.InitializeForm(user, deliveryAddress, invoiceAddress, list, GetIpAddress(), orderNumber);

            ViewBag.Token = checkOutForm.Token;
            ViewBag.Form = checkOutForm.CheckoutFormContent;

            TotalPrice(list);

            return View();
        }

        [Route("payment/credit-card")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult CreditCard(string delivery, string invoice)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            //Kredi karti entegrasyonu...
            //var checkOutForm = _iyzicoService.CreateForm()

            return View();
        }

        [Route("payment/transmitter")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Transmitter(string delivery, string invoice)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var userId = Utility.UserCheck().Id;
            var sessionId = GetSessionId();

            if (string.IsNullOrEmpty(delivery))
            {
                var list = _basketService.GetTopBasketList(sessionId, userId);
                ViewBag.Baskets = list;

                ErrorMessage("Lütfen önce teslimat adresini seçiniz.");
                return Redirect("/order/delivery-address");
            }

            if (string.IsNullOrEmpty(invoice))
            {
                var list = _basketService.GetTopBasketList(sessionId, userId);
                ViewBag.Baskets = list;

                ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
                ViewBag.DeliveryId = delivery;

                ErrorMessage("Fatura adresi seçmeniz zorunludur");
                return Redirect($"/order/invoice-address?delivery={delivery}");
            }

            var banks = _bankService.GetActiveBankList();
            ViewBag.Banks = banks;

            var payDoors = _cargoService.ActiveDoorPayList();
            ViewBag.PayDoors = payDoors;

            var defaultCargo = _cargoService.GetDefaultCargo();
            var defaultCargoAmount = defaultCargo.Price;
            var defaultCargoId = defaultCargo.Id;
            var orderNumber = _orderService.GetNewOrderNumber();

            var basketInfo = BasketInfo();

            var products = _basketService.GetUserBasketList(GetSessionId(), userId);
            var order = new Order
            {
                AddressId = delivery.ToInt32(),
                CargoId = defaultCargoId,
                CargoNr = "",
                CauseOfRefund = "",
                CreateDate = DateTime.Now,
                CreateUserId = userId,
                Description = "",
                DiscountAmount = products.BasketCampigns.DiscountTotalAmount,
                InvoiceId = invoice.ToInt32(),
                IpAddress = GetIpAddress(),
                IsCampaignApplied = products.BasketCampigns.DiscountTotalAmount != 0,
                KdvAmount = products.BasketTotalSum.KdvTotalAmount,
                OrderNote = basketInfo.OrderNote,
                OrderNr = orderNumber,
                OrderType = OrderTypes.Pending,
                OtherDetail = "",
                ParentOrderId = 0,
                PayType = PayTypes.Remittance,
                TotalAmount = products.BasketTotalSum.TotalAmount,
                UpdateDate = DateTime.Now,
                UpdateUserId = 0,
                UserId = userId
            };

            var orderProducts = products.ProductList.Select(a => new OrderProductAssg
            {
                CargoAmount = defaultCargoAmount,
                CauseOfRefund = "",
                DiscountAmount = a.DiscountAmount,
                DiscountOdd = a.DiscountOdd,
                IsCampaignApplied = a.IsCampaignApplied,
                IsKdv = a.IsKdv,
                KdvAmount = a.KdvAmount,
                KdvOdd = a.KdvOdd.ToInt32(),
                OrderId = 0,
                OrderType = OrderTypes.Pending,
                OrginalTotalAmount = a.OrginalTotalAmount,
                OtherDetail = Tool.ProductDetailSerialization(a.Detail),
                Price = a.ProductPrice,
                ProductId = a.ProductId,
                PurchasePrice = a.PurchasePrice,
                SubTotalAmount = a.SubTotalAmount,
                TotalAmount = a.TotalAmount,
                Unit = a.Unit
            }).ToList();

            try
            {
                _orderService.AddTransmitterOrder(order, orderProducts);
                _basketService.UserDeleteBaskets(userId);
                TempData["OrderNumber"] = orderNumber;
            }
            catch (Exception ex)
            {
                ErrorMessage($"Siparişiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyiniz. Hata Kodu: {ex.Message}");

                return Redirect($"/order/payment?delivery={delivery}&invoice={invoice}");
            }

            return Redirect("/order/payment/other-success");
        }

        [Route("payment/pay-door")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult PayDoor(string delivery, string invoice)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var userId = Utility.UserCheck().Id;

            if (string.IsNullOrEmpty(delivery))
            {
                var sessionId = GetSessionId();
                var list = _basketService.GetTopBasketList(sessionId, userId);
                ViewBag.Baskets = list;

                ErrorMessage("Lütfen önce teslimat adresini seçiniz.");
                return Redirect("/order/delivery-address");
            }

            if (string.IsNullOrEmpty(invoice))
            {
                var sessionId = GetSessionId();
                var list = _basketService.GetTopBasketList(sessionId, userId);
                ViewBag.Baskets = list;

                ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);
                ViewBag.DeliveryId = delivery;

                ErrorMessage("Fatura adresi seçmeniz zorunludur");
                return Redirect($"/order/invoice-address?delivery={delivery}");
            }

            var banks = _bankService.GetActiveBankList();
            ViewBag.Banks = banks;

            var payDoors = _cargoService.ActiveDoorPayList();
            ViewBag.PayDoors = payDoors;

            var defaultCargo = _cargoService.GetDefaultCargo();
            var defaultCargoAmount = defaultCargo.Price;
            var defaultCargoId = defaultCargo.Id;
            var orderNumber = _orderService.GetNewOrderNumber();

            var basketInfo = BasketInfo();

            var products = _basketService.GetUserBasketList(GetSessionId(), userId);
            var order = new Order
            {
                AddressId = delivery.ToInt32(),
                CargoId = defaultCargoId,
                CargoNr = "",
                CauseOfRefund = "",
                CreateDate = DateTime.Now,
                CreateUserId = userId,
                Description = "",
                DiscountAmount = products.BasketCampigns.DiscountTotalAmount,
                InvoiceId = invoice.ToInt32(),
                IpAddress = GetIpAddress(),
                IsCampaignApplied = products.BasketCampigns.DiscountTotalAmount != 0,
                KdvAmount = products.BasketTotalSum.KdvTotalAmount,
                OrderNote = basketInfo.OrderNote,
                OrderNr = orderNumber,
                OrderType = OrderTypes.Pending,
                OtherDetail = "",
                ParentOrderId = 0,
                PayType = PayTypes.PayDoor,
                TotalAmount = products.BasketTotalSum.TotalAmount,
                UpdateDate = DateTime.Now,
                UpdateUserId = 0,
                UserId = userId
            };

            var orderProducts = products.ProductList.Select(a => new OrderProductAssg
            {
                CargoAmount = defaultCargoAmount,
                CauseOfRefund = "",
                DiscountAmount = a.DiscountAmount,
                DiscountOdd = a.DiscountOdd,
                IsCampaignApplied = a.IsCampaignApplied,
                IsKdv = a.IsKdv,
                KdvAmount = a.KdvAmount,
                KdvOdd = a.KdvOdd.ToInt32(),
                OrderId = 0,
                OrderType = OrderTypes.Pending,
                OrginalTotalAmount = a.OrginalTotalAmount,
                OtherDetail = Tool.ProductDetailSerialization(a.Detail),
                Price = a.ProductPrice,
                ProductId = a.ProductId,
                PurchasePrice = a.PurchasePrice,
                SubTotalAmount = a.SubTotalAmount,
                TotalAmount = a.TotalAmount,
                Unit = a.Unit
            }).ToList();

            try
            {
                _orderService.AddTransmitterOrder(order, orderProducts);
                _basketService.UserDeleteBaskets(userId);
                TempData["OrderNumber"] = orderNumber;
            }
            catch (Exception ex)
            {
                ErrorMessage($"Siparişiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyiniz. Hata Kodu: {ex.Message}");

                return Redirect($"/order/payment?delivery={delivery}&invoice={invoice}");
            }

            return Redirect("/order/payment/other-success");
        }

        [Route("payment/success")]
        [HttpPost]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult OrderSuccess(string token)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            var basketInfo = BasketInfo();

            if (string.IsNullOrEmpty(token))
            {
                ErrorMessage("Bana öyle bakma anlayacaklar...");
                return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
            }

            var formCheck = _iyzicoService.RequestForm(token);
            if (formCheck.Status.ToLower() == "success" && formCheck.PaymentStatus.ToLower() == "success")
            {
                //satis islemi gerceklesecek..
                var userId = Utility.UserCheck().Id;

                var defaultCargo = _cargoService.GetDefaultCargo();
                var defaultCargoAmount = defaultCargo.Price;
                var defaultCargoId = defaultCargo.Id;
                var orderNumber = _orderService.GetNewOrderNumber();

                var products = _basketService.GetUserBasketList(GetSessionId(), userId);
                var order = new Order
                {
                    AddressId = basketInfo.DeliveryId.ToInt32(),
                    CargoId = defaultCargoId,
                    CargoNr = "",
                    CauseOfRefund = "",
                    CreateDate = DateTime.Now,
                    CreateUserId = userId,
                    Description = "",
                    DiscountAmount = products.BasketCampigns.DiscountTotalAmount,
                    InvoiceId = basketInfo.InvoiceId.ToInt32(),
                    IpAddress = GetIpAddress(),
                    IsCampaignApplied = products.BasketCampigns.DiscountTotalAmount != 0,
                    KdvAmount = products.BasketTotalSum.KdvTotalAmount,
                    OrderNote = basketInfo.OrderNote,
                    OrderNr = orderNumber,
                    OrderType = OrderTypes.Pending,
                    OtherDetail = "",
                    ParentOrderId = 0,
                    PayType = PayTypes.CreditCard,
                    TotalAmount = products.BasketTotalSum.TotalAmount,
                    UpdateDate = DateTime.Now,
                    UpdateUserId = 0,
                    UserId = userId
                };

                var orderProducts = products.ProductList.Select(a => new OrderProductAssg
                {
                    CargoAmount = defaultCargoAmount,
                    CauseOfRefund = "",
                    DiscountAmount = a.DiscountAmount,
                    DiscountOdd = a.DiscountOdd,
                    IsCampaignApplied = a.IsCampaignApplied,
                    IsKdv = a.IsKdv,
                    KdvAmount = a.KdvAmount,
                    KdvOdd = a.KdvOdd.ToInt32(),
                    OrderId = 0,
                    OrderType = OrderTypes.Pending,
                    OrginalTotalAmount = a.OrginalTotalAmount,
                    OtherDetail = Tool.ProductDetailSerialization(a.Detail),
                    Price = a.ProductPrice,
                    ProductId = a.ProductId,
                    PurchasePrice = a.PurchasePrice,
                    SubTotalAmount = a.SubTotalAmount,
                    TotalAmount = a.TotalAmount,
                    Unit = a.Unit
                }).ToList();

                try
                {
                    _orderService.AddTransmitterOrder(order, orderProducts);
                    _basketService.UserDeleteBaskets(userId);
                    TempData["OrderNumber"] = orderNumber;
                }
                catch (Exception ex)
                {
                    ErrorMessage($"Siparişiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyiniz. Hata Kodu: {ex.Message} <br><br><br>Ödemeniz gerçekleşti fakat ürünleriniz satışa alınamadı. Lütfen bize ulaşarak durum hakkında bilgi alıp/ bilgilendiriniz.");

                    return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
                }
            }
            else
            {
                if (formCheck.ErrorGroup != null)
                {
                    var errorMessage = string.Empty;
                    var checkMessage = Tool.GetCreditCardErrorDescription.TryGetValue(formCheck.ErrorGroup, out errorMessage);
                    if (checkMessage)
                    {
                        var errorDescription = Tool.GetCreditCardErrorDescription?[formCheck.ErrorGroup];
                        ErrorMessage($"Ödeme alınamadı. Hata detayı:{errorDescription}");
                        return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
                    }

                    ErrorMessage($"Ödeme alınamadı. Hata detayı: Lütfen bilgilerinizi kontrol ediniz.");
                    return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
                }

                var paymentErrorMessage = Tool.GetPaymentErrorDescription[formCheck.PaymentStatus];
                ErrorMessage($"Ödeme alınamadı. Hata detayı:{paymentErrorMessage}");
                return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
            }

            return View();
        }

        [Route("payment/other-success")]
        [HttpGet]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult OtherOrderSuccess()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login?returnUrl=/basket");

            //var basketInfo = BasketInfo();

            //var userId = Utility.UserCheck().Id;

            //var defaultCargo = _cargoService.GetDefaultCargo();
            //var defaultCargoAmount = defaultCargo.Price;
            //var defaultCargoId = defaultCargo.Id;
            //var orderNumber = _orderService.GetNewOrderNumber();

            //var products = _basketService.GetUserBasketList(GetSessionId(), userId);
            //var order = new Order
            //{
            //    AddressId = basketInfo.DeliveryId.ToInt32(),
            //    CargoId = defaultCargoId,
            //    CargoNr = "",
            //    CauseOfRefund = "",
            //    CreateDate = DateTime.Now,
            //    CreateUserId = userId,
            //    Description = "",
            //    DiscountAmount = products.BasketCampigns.DiscountTotalAmount,
            //    InvoiceId = basketInfo.InvoiceId.ToInt32(),
            //    IpAddress = GetIpAddress(),
            //    IsCampaignApplied = products.BasketCampigns.DiscountTotalAmount != 0,
            //    KdvAmount = products.BasketTotalSum.KdvTotalAmount,
            //    OrderNote = "",
            //    OrderNr = orderNumber,
            //    OrderType = OrderTypes.Pending,
            //    OtherDetail = "",
            //    ParentOrderId = 0,
            //    PayType = PayTypes.PayDoor,
            //    TotalAmount = products.BasketTotalSum.TotalAmount,
            //    UpdateDate = DateTime.Now,
            //    UpdateUserId = 0,
            //    UserId = userId
            //};

            //var orderProducts = products.ProductList.Select(a => new OrderProductAssg
            //{
            //    CargoAmount = defaultCargoAmount,
            //    CauseOfRefund = "",
            //    DiscountAmount = a.DiscountAmount,
            //    DiscountOdd = a.DiscountOdd,
            //    IsCampaignApplied = a.IsCampaignApplied,
            //    IsKdv = a.IsKdv,
            //    KdvAmount = a.KdvAmount,
            //    KdvOdd = a.KdvOdd.ToInt32(),
            //    OrderId = 0,
            //    OrderType = OrderTypes.Pending,
            //    OrginalTotalAmount = a.OrginalTotalAmount,
            //    OtherDetail = Tool.ProductDetailSerialization(a.Detail),
            //    Price = a.ProductPrice,
            //    ProductId = a.ProductId,
            //    PurchasePrice = a.PurchasePrice,
            //    SubTotalAmount = a.SubTotalAmount,
            //    TotalAmount = a.TotalAmount,
            //    Unit = a.Unit
            //}).ToList();

            //try
            //{
            //    _orderService.AddTransmitterOrder(order, orderProducts);
            //    _basketService.UserDeleteBaskets(userId);
            //    TempData["OrderNumber"] = orderNumber;
            //}
            //catch (Exception ex)
            //{
            //    ErrorMessage($"Siparişiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyiniz. Hata Kodu: {ex.Message} <br><br><br>Ödemeniz gerçekleşti fakat ürünleriniz satışa alınamadı. Lütfen bize ulaşarak durum hakkında bilgi alıp/ bilgilendiriniz.");

            //    return Redirect($"/order/payment?delivery={basketInfo.DeliveryId}&invoice={basketInfo.InvoiceId}");
            //}

            return View();
        }

        private void TotalPrice(BasketListDto list)
        {
            var price = list.BasketTotalSum.TotalAmount;
            var cargoDiscount = list.BasketCampigns.DiscountItems.FirstOrDefault(
                    a => a.CampaignType == CampaignType.Cargo);
            if (cargoDiscount != null)
            {
                price = price - list.BasketTotalSum.CargoAmount;
            }
            else
            {
                price = price + list.BasketTotalSum.CargoAmount;
            }
            ViewBag.Payment = price.ToString("n2");
            ViewBag.PaymentPrice = price;
        }
    }
}