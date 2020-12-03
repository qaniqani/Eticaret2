using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Helpers;
using AdminProject.Attributes;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;
using AdminProject.Services.CustomExceptions;

namespace AdminProject.Controllers
{
    [RoutePrefix("user")]
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ICountryService _countryService;
        private readonly ICityService _cityService;
        private readonly IRegionService _regionService;
        private readonly IAddressService _addressService;
        private readonly IInvoiceService _invoiceService;
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;
        private readonly IFavoriteService _favoriteService;

        public LoginController(IUserService userService, ICountryService countryService, ICityService cityService,
            IRegionService regionService, IAddressService addressService, IInvoiceService invoiceService,
            IBasketService basketService, IOrderService orderService, IFavoriteService favoriteService)
        {
            _userService = userService;
            _countryService = countryService;
            _cityService = cityService;
            _regionService = regionService;
            _addressService = addressService;
            _invoiceService = invoiceService;
            _basketService = basketService;
            _orderService = orderService;
            _favoriteService = favoriteService;
        }

        [HttpGet]
        [Route("login")]
        [CookieCheck]
        public ActionResult Login()
        {
            if (Utility.UserCheck() != null)
                return Redirect("/user/my-account");

            return View();
        }

        [HttpGet]
        [Route("logout")]
        public ActionResult LogOut()
        {
            Response.Cookies.Clear();
            var c = new HttpCookie("User") {Expires = DateTime.Now.AddDays(-1)};
            Response.Cookies.Add(c);

            Session.Remove("User");
            Session.Clear();
            Session.Abandon();

            return Redirect("/");
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login(UserLoginDto request, string returnUrl)
        {
            if (string.IsNullOrEmpty(request.Email))
                ModelState.AddModelError("Email", "Email adresi zorunludur");

            if (string.IsNullOrEmpty(request.Password))
                ModelState.AddModelError("Password", "Şifre zorunludur");

            if (!ModelState.IsValid)
            {
                var errors = GetErrorMessage(ModelState.Values);
                ErrorMessage(errors);

                return View(request);
            }

            try
            {
                var user = _userService.Login(request.Email, request.Password);

                var sessionId = GetSessionId();
                _basketService.SessionSetLoginUser(sessionId, user.Id);

                Session["User"] = user;

                if (request.IsRememberMe)
                {
                    var cookie = new HttpCookie("User")
                    {
                        Expires = DateTime.Now.AddMonths(12)
                    };
                    cookie.Values["Email"] = user.Email;

                    Response.Cookies.Add(cookie);
                }
            }
            catch (CustomException ex)
            {
                ErrorMessage(ex.Message);
                return View(request);
            }

            var redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/basket" : returnUrl;

            return Redirect(redirectUrl);
        }

        [HttpGet]
        [Route("create")]
        public ActionResult Create()
        {
            ViewBag.Gender = DropdownTypes.GetGenderType("Bay");

            return View(new UserCreateRequestDto());
        }

        [HttpGet]
        [Route("forgot-password")]
        [CookieCheck]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [Route("forgot-password")]
        public ActionResult ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ErrorMessage("E-Mail adresi boş bırakılamaz.");
                return View(email);
            }

            _userService.SendForgotPassword(email);

            SuccessMessage("Şifreniz mail adresinize gönderilmiştir.");

            return Redirect("/");
        }

        [HttpPost]
        [Route("create")]
        public ActionResult Create(UserCreateRequestDto request)
        {
            ViewBag.Gender = DropdownTypes.GetGenderType(request.Gender);

            if (string.IsNullOrEmpty(request.Email))
                ModelState.AddModelError("Email", "E-Mail zorunludur.");

            if (string.IsNullOrEmpty(request.Gender))
                ModelState.AddModelError("Gender", "Cinsiyet zorunludur.");

            if (string.IsNullOrEmpty(request.Gsm))
                ModelState.AddModelError("Gsm", "Gsm zorunludur.");

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "İsim zorunludur.");

            if (string.IsNullOrEmpty(request.Password))
                ModelState.AddModelError("Password", "Şifre zorunludur.");

            if (string.IsNullOrEmpty(request.PasswordAgain))
                ModelState.AddModelError("PasswordAgain", "Şifre tekrarı zorunludur.");

            if (string.IsNullOrEmpty(request.Phone))
                ModelState.AddModelError("Phone", "Telefon zorunludur.");

            if (string.IsNullOrEmpty(request.Surname))
                ModelState.AddModelError("Surname", "Soyisim zorunludur.");

            if (string.IsNullOrEmpty(request.TcNr))
                ModelState.AddModelError("TcNr", "T.C. Kimlik no zorunludur.");

            if (!ModelState.IsValid)
            {
                var errors = GetErrorMessage(ModelState.Values);
                ErrorMessage(errors);
                return View(request);
            }

            if (_userService.EmailCheck(request.Email))
            {
                ErrorMessage("E-Mail daha önce kullanılmış.");
                return View(request);
            }

            if (request.Password != request.PasswordAgain)
            {
                ErrorMessage("Şifreler uyuşmuyor.");
                return View(request);
            }

            const string country = "TURKIYE";
            const string city = "ANKARA";
            const string region = "ÇANKAYA";

            var user = new User
            {
                ActivationCode = Guid.NewGuid().ToString(),
                Address = "",
                BannedMessage = "",
                BirthDate2 = new DateTime(1970, 1, 1),
                City = city,
                Country = country,
                CreateDate = DateTime.Now,
                Email = request.Email,
                Gender = request.Gender,
                Gsm = request.Gsm,
                LastLoginDate = new DateTime(1970, 1, 1),
                Name = request.Name,
                Password = request.Password,
                Phone = request.Phone,
                Region = region,
                Status = UserTypes.Deactive,
                Surname = request.Surname,
                TcNr = request.TcNr,
                UpdateDate = DateTime.Now
            };

            try
            {
                _userService.Add(user);
                SuccessMessage(
                "Kullanıcı kaydınız başarıyla gerçekleştirilmiştir. Lütfen E-Mail adresinize gönderilen (spam kutunuzu da kontrol ediniz) aktivasyon kodu ile hesabınızı aktif ediniz.");

                return Redirect("/");
            }
            catch (CustomException ex)
            {
                ViewBag.Gender = DropdownTypes.GetGenderType("Bay");
                ErrorMessage(ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Gender = DropdownTypes.GetGenderType("Bay");
                ErrorMessage($"Bir hata oluştu. Hata Detayı: {ex.Message}");
                _userService.Delete(user.Email);
                return View();
            }
        }

        [HttpGet]
        [Route("activation/{activationCode}")]
        public ActionResult Activation(string activationCode)
        {
            if (string.IsNullOrEmpty(activationCode))
            {
                ErrorMessage("Aktivasyon kodu boş bırakılamaz!..");
                return Redirect("/");
            }

            var result = _userService.EmailActivation(activationCode);
            if (result)
            {
                SuccessMessage("Üyeliğiniz aktif edilmiştir. Giriş yapabilirsiniz.");
                return Redirect("/user/login");
            }

            ErrorMessage("İlgili kullanıcı bulunamamıştır. Lütfen bilgilerinizi kontrol ediniz.");
            return Redirect("/");
        }

        [HttpGet]
        [Route("my-account")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Account()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var result = _userService.GetUserDetail(userId);

            ViewBag.Gender = DropdownTypes.GetGenderType(result.Gender);

            var countries = _countryService.GetCountrySelectList("0");

            ViewBag.CountryList = new SelectList(countries, "Value", "Text", result.Country);

            return View(result);
        }

        [HttpGet]
        [Route("my-account/edit")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Edit()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var result = _userService.GetUserDetail(userId);

            ViewBag.Gender = DropdownTypes.GetGenderType(result.Gender);

            var countries = _countryService.GetCountrySelectList("0");

            ViewBag.CountryList = new SelectList(countries, "Value", "Text", result.Country);

            return View(result);
        }

        [HttpPost]
        [Route("my-account/edit")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Edit(int Id, string Name, string Surname, string TcNr, string Gender, string Email,
            string Phone, string Gsm, string Country, string City, string Region, string Address, string Password,
            string NewPassword, string NewPassword2)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var result = _userService.GetUserDetail(Id);

            ViewBag.Gender = DropdownTypes.GetGenderType(Gender);

            var countries = _countryService.GetCountrySelectList("0");

            ViewBag.CountryList = new SelectList(countries, "Value", "Text", Country);

            if (string.IsNullOrEmpty(Name))
                ModelState.AddModelError("Name", "İsim zorunludur");

            if (string.IsNullOrEmpty(Surname))
                ModelState.AddModelError("Surname", "Soyisim zorunludur");

            if (string.IsNullOrEmpty(TcNr))
                ModelState.AddModelError("TcNr", "T.C. kimlik zorunludur");

            if (string.IsNullOrEmpty(Gender))
                ModelState.AddModelError("Gender", "Cinsiye zorunludur");

            if (string.IsNullOrEmpty(Email))
                ModelState.AddModelError("Email", "Email zorunludur");

            if (string.IsNullOrEmpty(Phone))
                ModelState.AddModelError("Phone", "Telefon zorunludur");

            if (string.IsNullOrEmpty(Gsm))
                ModelState.AddModelError("Gsm", "Gsm zorunludur");

            if (string.IsNullOrEmpty(Country))
                ModelState.AddModelError("Country", "Ülke zorunludur");

            if (string.IsNullOrEmpty(City))
                ModelState.AddModelError("City", "İl zorunludur");

            if (string.IsNullOrEmpty(Region))
                ModelState.AddModelError("Region", "İlçe zorunludur");

            if (string.IsNullOrEmpty(Address))
                ModelState.AddModelError("Address", "Adres zorunludur");

            if (string.IsNullOrEmpty(Password))
                ModelState.AddModelError("Password", "Şifre zorunludur");

            if (Password != result.Password)
                ModelState.AddModelError("PasswordMatch", "Kullanıcı şifresi hatalı");

            if (!ModelState.IsValid)
            {
                ErrorMessage(GetErrorMessage(ModelState.Values));
                return View(result);
            }

            var password = Password;
            if (!string.IsNullOrEmpty(NewPassword) || !string.IsNullOrEmpty(NewPassword2))
            {
                if (NewPassword == NewPassword2)
                    password = NewPassword;
                else
                    ModelState.AddModelError("NewPasswordError", "Yeni şifre uyuşmuyor");
            }

            if (!ModelState.IsValid)
            {
                ErrorMessage(GetErrorMessage(ModelState.Values));
                return View(result);
            }

            var user = _userService.GetUser(Id);
            user.Address = Address;
            user.City = City;
            user.Country = Country;
            user.Email = Email;
            user.Gender = Gender;
            user.Gsm = Gsm;
            user.Name = Name;
            user.Password = password;
            user.Phone = Phone;
            user.Region = Region;
            user.Surname = Surname;
            user.TcNr = TcNr;
            user.UpdateDate = DateTime.Now;

            _userService.Edit(Id, user);

            ViewBag.Success = "Bilgileriniz kaydedildi.";

            return View(result);
        }

        [HttpGet]
        [Route("address/delivery")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Delivery()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var result = _addressService.GetUserAddressUserActiveList(userId);

            ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);

            return View(result);
        }

        [HttpGet]
        [Route("address/get-delivery")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Delivery(string id)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var deliveryId = id.ToInt32();
            var delivery = _addressService.GetAddress(deliveryId, userId);
            if (delivery == null)
            {
                Response.StatusCode = 400;
                return Json("Adres bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            return Json(delivery, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("address/delete-delivery")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult DeliveryFreeze(string id)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var deliveryId = id.ToInt32();
            _addressService.Freeze(deliveryId);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("address/set-delivery")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult EditDelivery(string id, string addressSaveName, string nameSurname, string addressDetail,
            string city, string region, string phone, string gsm, string tcNr, string addressNote, string status)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("id", "Adres seçiniz");

            if (string.IsNullOrEmpty(addressSaveName))
                ModelState.AddModelError("addressSaveName", "Adres adı zorunludur");

            if (string.IsNullOrEmpty(nameSurname))
                ModelState.AddModelError("nameSurname", "İsim, soyisim zorunludur");

            if (string.IsNullOrEmpty(addressDetail))
                ModelState.AddModelError("addressDetail", "Adres detayı zorunludur");

            if (string.IsNullOrEmpty(city))
                ModelState.AddModelError("city", "İl zorunludur");

            if (string.IsNullOrEmpty(region))
                ModelState.AddModelError("region", "İlçe zorunludur");

            if (string.IsNullOrEmpty(phone))
                ModelState.AddModelError("phone", "Telefon zorunludur");

            if (string.IsNullOrEmpty(gsm))
                ModelState.AddModelError("gsm", "Gsm zorunludur");

            if (string.IsNullOrEmpty(tcNr))
                ModelState.AddModelError("tcNr", "T.C. Kimlik no zorunludur");

            if (string.IsNullOrEmpty(status))
                ModelState.AddModelError("status", "Durum zorunludur");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var deliveryId = id.ToInt32();
            var userId = Utility.UserCheck().Id;
            var delivery = _addressService.GetAddress(deliveryId, userId);
            if (delivery == null)
            {
                Response.StatusCode = 400;
                return Json("Adres bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            delivery.AddressDetail = addressDetail;
            delivery.AddressNote = addressNote;
            delivery.AddressSaveName = addressSaveName;
            delivery.City = city;
            delivery.Gsm = gsm;
            delivery.NameSurname = nameSurname;
            delivery.Phone = phone;
            delivery.Region = region;
            delivery.Status = (StatusTypes) status.ToInt32();
            delivery.TcNr = tcNr;
            delivery.UpdateDate = DateTime.Now;

            _addressService.Edit(deliveryId, delivery);

            return Json("Teslimat adresi güncellendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("address/add-delivery")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult AddDelivery(string addressSaveName, string nameSurname, string addressDetail, string city,
            string region, string phone, string gsm, string tcNr, string addressNote, string status)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            if (string.IsNullOrEmpty(addressSaveName))
                ModelState.AddModelError("addressSaveName", "Adres adı zorunludur");

            if (string.IsNullOrEmpty(nameSurname))
                ModelState.AddModelError("nameSurname", "İsim, soyisim zorunludur");

            if (string.IsNullOrEmpty(addressDetail))
                ModelState.AddModelError("addressDetail", "Adres detayı zorunludur");

            if (string.IsNullOrEmpty(city))
                ModelState.AddModelError("city", "İl zorunludur");

            if (string.IsNullOrEmpty(region))
                ModelState.AddModelError("region", "İlçe zorunludur");

            if (string.IsNullOrEmpty(phone))
                ModelState.AddModelError("phone", "Telefon zorunludur");

            if (string.IsNullOrEmpty(gsm))
                ModelState.AddModelError("gsm", "Gsm zorunludur");

            if (string.IsNullOrEmpty(tcNr))
                ModelState.AddModelError("tcNr", "T.C. Kimlik no zorunludur");

            if (string.IsNullOrEmpty(status))
                ModelState.AddModelError("status", "Durum zorunludur");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var userId = Utility.UserCheck().Id;
            var delivery = new Address
            {
                UserId = userId,
                CreateDate = DateTime.Now,
                AddressDetail = addressDetail,
                AddressNote = addressNote,
                AddressSaveName = addressSaveName,
                City = city,
                Gsm = gsm,
                NameSurname = nameSurname,
                Phone = phone,
                Region = region,
                Status = (StatusTypes) status.ToInt32(),
                TcNr = tcNr,
                UpdateDate = DateTime.Now
            };

            _addressService.Add(delivery);

            return Json("Yeni teslimat adresi eklendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("address/invoice")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Invoice()
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var result = _invoiceService.GetUserInoive(userId);

            ViewBag.Status = DropdownTypes.GetUserStatus(StatusTypes.Active);

            return View(result);
        }

        [HttpGet]
        [Route("address/get-invoice")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Invoice(string id)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var userId = Utility.UserCheck().Id;
            var invoiceId = id.ToInt32();
            var invoice = _invoiceService.GetInvoice(invoiceId, userId);
            if (invoice == null)
            {
                Response.StatusCode = 400;
                return Json("Fatura bilgisi bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            return Json(invoice, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("address/delete-invoice")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult InvoiceFreeze(string id)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            var invoiceId = id.ToInt32();
            _invoiceService.Freeze(invoiceId);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("address/set-invoice")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult EditInvoice(string id, string invoiceSaveName, string nameSurname, string address,
            string country, string city, string region, string phone, string gsm, string taxNr, string taxOffice,
            string invoiceType, string status)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            if (string.IsNullOrEmpty(id))
                ModelState.AddModelError("id", "Adres seçiniz");

            if (string.IsNullOrEmpty(invoiceSaveName))
                ModelState.AddModelError("invoiceSaveName", "Fatura adı zorunludur");

            if (string.IsNullOrEmpty(nameSurname))
                ModelState.AddModelError("nameSurname", "İsim, soyisim zorunludur");

            if (string.IsNullOrEmpty(address))
                ModelState.AddModelError("addressDetail", "Adres detayı zorunludur");

            if (string.IsNullOrEmpty(country))
                ModelState.AddModelError("country", "Ülke zorunludur");

            if (string.IsNullOrEmpty(city))
                ModelState.AddModelError("city", "İl zorunludur");

            if (string.IsNullOrEmpty(region))
                ModelState.AddModelError("region", "İlçe zorunludur");

            if (string.IsNullOrEmpty(phone))
                ModelState.AddModelError("phone", "Telefon zorunludur");

            if (string.IsNullOrEmpty(gsm))
                ModelState.AddModelError("gsm", "Gsm zorunludur");

            if (string.IsNullOrEmpty(invoiceType))
                ModelState.AddModelError("invoiceType", "Fatura tipi zorunludur");

            if (string.IsNullOrEmpty(taxNr))
                ModelState.AddModelError("taxNr", "Vergi/ T.C. Kimlik no zorunludur");

            if (string.IsNullOrEmpty(status))
                ModelState.AddModelError("status", "Durum zorunludur");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var iType = (InoviceTypes) invoiceType.ToInt32();
            if (iType == InoviceTypes.Corporate)
            {
                if (string.IsNullOrEmpty(taxOffice))
                {
                    Response.StatusCode = 400;
                    return Json("Kurumsal fatura bilgilerinde vergi dairesi zorunludur.", JsonRequestBehavior.AllowGet);
                }
            }

            var invoiceId = id.ToInt32();
            var userId = Utility.UserCheck().Id;
            var invoice = _invoiceService.GetInvoice(invoiceId, userId);
            if (invoice == null)
            {
                Response.StatusCode = 400;
                return Json("Fatura adresi bulunamadı.", JsonRequestBehavior.AllowGet);
            }

            invoice.Address = address;
            invoice.InvoiceSaveName = invoiceSaveName;
            invoice.InvoiceType = (InoviceTypes) invoiceType.ToInt32();
            invoice.Country = country;
            invoice.City = city;
            invoice.Gsm = gsm;
            invoice.NameSurname = nameSurname;
            invoice.Phone = phone;
            invoice.Region = region;
            invoice.Status = (StatusTypes) status.ToInt32();
            invoice.TaxNr = taxNr;
            invoice.TaxOffice = taxOffice;
            invoice.IsEInvoice = false;

            _invoiceService.Edit(invoiceId, invoice);

            return Json("Fatura adresi güncellendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Route("address/add-invoice")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult AddInvoice(string invoiceSaveName, string nameSurname, string address, string country,
            string city, string region, string phone, string gsm, string taxNr, string taxOffice, string invoiceType,
            string status)
        {
            if (Utility.UserCheck() == null)
                return Redirect("/user/login");

            if (string.IsNullOrEmpty(invoiceSaveName))
                ModelState.AddModelError("invoiceSaveName", "Fatura adı zorunludur");

            if (string.IsNullOrEmpty(nameSurname))
                ModelState.AddModelError("nameSurname", "İsim, soyisim zorunludur");

            if (string.IsNullOrEmpty(address))
                ModelState.AddModelError("addressDetail", "Adres detayı zorunludur");

            if (string.IsNullOrEmpty(country))
                ModelState.AddModelError("country", "Ülke zorunludur");

            if (string.IsNullOrEmpty(city))
                ModelState.AddModelError("city", "İl zorunludur");

            if (string.IsNullOrEmpty(region))
                ModelState.AddModelError("region", "İlçe zorunludur");

            if (string.IsNullOrEmpty(phone))
                ModelState.AddModelError("phone", "Telefon zorunludur");

            if (string.IsNullOrEmpty(gsm))
                ModelState.AddModelError("gsm", "Gsm zorunludur");

            if (string.IsNullOrEmpty(invoiceType))
                ModelState.AddModelError("invoiceType", "Fatura tipi zorunludur");

            if (string.IsNullOrEmpty(taxNr))
                ModelState.AddModelError("taxNr", "Vergi/ T.C. Kimlik no zorunludur");

            if (string.IsNullOrEmpty(status))
                ModelState.AddModelError("status", "Durum zorunludur");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var iType = (InoviceTypes) invoiceType.ToInt32();
            if (iType == InoviceTypes.Corporate)
            {
                if (string.IsNullOrEmpty(taxOffice))
                {
                    Response.StatusCode = 400;
                    return Json("Kurumsal fatura bilgilerinde vergi dairesi zorunludur.", JsonRequestBehavior.AllowGet);
                }
            }

            var userId = Utility.UserCheck().Id;
            var invoice = new Invoice
            {
                UserId = userId,
                City = city,
                Gsm = gsm,
                NameSurname = nameSurname,
                Phone = phone,
                Region = region,
                Status = (StatusTypes) status.ToInt32(),
                Address = address,
                Country = country,
                InvoiceSaveName = invoiceSaveName,
                InvoiceType = iType,
                IsEInvoice = false,
                TaxNr = taxNr,
                TaxOffice = taxOffice
            };

            _invoiceService.Add(invoice);

            return Json("Yeni fatura adresi eklendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("city")]
        [CookieCheck]
        [AuthorizationFilter]
        public JsonResult GetCity(int countryId)
        {
            if (countryId == 0)
                return Json(new {Name = "Ülke Seç", Id = 0}, JsonRequestBehavior.AllowGet);

            var cities =
                _cityService.AllCityList(countryId).OrderBy(a => a.SequenceNr).Select(a => new {a.Name, a.Id}).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("city/region")]
        [CookieCheck]
        [AuthorizationFilter]
        public JsonResult GetRegion(int cityId)
        {
            if (cityId == 0)
                return Json(new {Name = "İl Seç", Id = 0}, JsonRequestBehavior.AllowGet);

            var cities =
                _regionService.AllRegionList(cityId).OrderBy(a => a.SequenceNr).Select(a => new {a.Name, a.Id}).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("orders")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Order()
        {
            var userId = Utility.UserCheck().Id;
            var userOrders = _orderService.GetUserOrders(userId);

            return View(userOrders);
        }

        [HttpGet]
        [Route("order/{id}/detail")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult OrderDetail(int id)
        {
            var userId = Utility.UserCheck().Id;
            var products = _orderService.GetUserOrderDetail(userId, id);

            return View("OrderDetail", products);
        }

        [HttpGet]
        [Route("ticket")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Ticket()
        {
            ViewBag.User = Utility.UserCheck();

            return View(new TicketDto());
        }

        [HttpPost]
        [Route("ticket")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Ticket(TicketDto request)
        {
            var user = Utility.UserCheck();
            ViewBag.User = user;

            try
            {
                _userService.SendEftNotificationForm(user.Email, user.Name, user.Surname, request.OrderNr, request.Message);

                SuccessMessage("Havale bildirim formunuz başarıyla tarafımıza ulaşmıştır.");
                return Redirect("/user/my-account");
            }
            catch (Exception)
            {
                ErrorMessage("Form gönderimi başarısız oldu. Lütfen bir süre sonra tekrar deneyiniz.");
                return View(request);
            }
        }

        [HttpGet]
        [Route("favorites")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Favorites()
        {
            var userId = Utility.UserCheck().Id;

            var favorites = _favoriteService.GetList(userId);

            return View(favorites);
        }

        [HttpPost]
        [Route("favorite")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult Favorite(string productName, string productUrl, string productId)
        {
            if (Utility.UserCheck() == null)
            {
                Response.StatusCode = 400;
                return Json("Lütfen önce üye girişi yapınız.", JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(productName))
                ModelState.AddModelError("ProductName", "Ürün seçiniz.");

            if (string.IsNullOrEmpty(productUrl))
                ModelState.AddModelError("ProductUrl", "Ürün belirtiniz.");

            if (string.IsNullOrEmpty(productId))
                ModelState.AddModelError("ProductId", "Ürün tanımlayınız.");

            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrorMessage(ModelState.Values);
                return Json(errors, JsonRequestBehavior.AllowGet);
            }

            var userId = Utility.UserCheck().Id;

            var favorite = new Favorite
            {
                DateTime = DateTime.Now,
                ProductId = productId.ToInt32(),
                ProductName = productName,
                ProductUrl = productUrl,
                UserId = userId
            };

            _favoriteService.Add(favorite);

            return Json($"{productName} favorilerinize eklendi.", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("favorite/{id}/delete")]
        [CookieCheck]
        [AuthorizationFilter]
        public ActionResult FavoriteDelete(int id)
        {
            var userId = Utility.UserCheck().Id;

            _favoriteService.Delete(userId, id);

            SuccessMessage("Seçilen ürün favorilerden kaldırıldı");

            return Redirect("/user/favorites");
        }
    }
}