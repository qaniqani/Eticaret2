using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;

namespace AdminProject.Services
{
    public class IyzipayService : IIyzipayService
    {
        private readonly Options _option;
        private readonly IyzipayConfig _iyzicoConfig;

        public IyzipayService(IyzipayConfig iyzicoConfig)
        {
            _iyzicoConfig = iyzicoConfig;
            _option = new Options
            {
                ApiKey = iyzicoConfig.ApiKey,
                BaseUrl = iyzicoConfig.BaseUrl,
                SecretKey = iyzicoConfig.SecretKey
            };
        }

        public CheckoutFormInitialize InitializeForm(Infrastructure.Models.User user, Infrastructure.Models.Address deliveryAddress, Infrastructure.Models.Invoice invoiceAddress, BasketListDto basketItems, string ipAddress, string orderNumber)
        {
            var buyer = new Buyer
            {
                City = invoiceAddress.City,
                Country = invoiceAddress.Country,
                Email = user.Email,
                GsmNumber = user.Gsm,
                Id = $"BY{user.Id}",
                IdentityNumber = invoiceAddress.TaxNr,
                Ip = ipAddress,
                LastLoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                Name = user.Name,
                RegistrationAddress = invoiceAddress.Address,
                RegistrationDate = user.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Surname = user.Surname
            };

            var shippingAddress = new Address
            {
                City = deliveryAddress.City,
                ContactName = deliveryAddress.NameSurname,
                Country = "TURKIYE",
                Description = deliveryAddress.AddressDetail
            };

            var billingAddress = new Address
            {
                City = invoiceAddress.City,
                ContactName = invoiceAddress.NameSurname,
                Country = invoiceAddress.Country,
                Description = invoiceAddress.Address,
                ZipCode = "34000"
            };

            List<BasketItem> newBasketItems;
            if (basketItems.BasketCampigns.DiscountTotalAmount > 0)
            {
                var discountOdd = basketItems.BasketCampigns.DiscountOdd;

                newBasketItems = basketItems.ProductList.Select(a =>
                {
                    var discountedAmount = a.TotalAmount - a.TotalAmount / 100 * discountOdd;

                    var item = new BasketItem
                    {
                        Category1 = "Takı",
                        Id = $"BI{a.BasketId}",
                        ItemType = BasketItemType.PHYSICAL.ToString(),
                        Name = a.ProductName,
                        Price = discountedAmount.ToString("0.00", CultureInfo.InvariantCulture)
                    };

                    return item;
                }).ToList();
            }
            else
            {
                newBasketItems = basketItems.ProductList.Select(a => new BasketItem
                {
                    Category1 = "Takı",
                    Id = $"BI{a.BasketId}",
                    ItemType = BasketItemType.PHYSICAL.ToString(),
                    Name = a.ProductName,
                    Price = a.TotalAmount.ToString("0.00", CultureInfo.InvariantCulture)
                }).ToList();
            }

            var enabledInstallments = new List<int> { 2, 3, 6, 9 };

            var conversionId = DateTime.Now.Ticks.ToString().Substring(0, 9);

            var paidPrice = basketItems.BasketTotalSum.TotalAmount;
            var price = basketItems.BasketTotalSum.TotalAmount;
            var cargoDiscount =
                basketItems.BasketCampigns.DiscountItems.FirstOrDefault(
                    a => a.CampaignType == Infrastructure.Models.CampaignType.Cargo);
            if (cargoDiscount != null)
            {
                price = price - basketItems.BasketTotalSum.CargoAmount;
            }
            else
            {
                paidPrice = paidPrice + basketItems.BasketTotalSum.CargoAmount;
            }

            var paymentRequest = new CreateCheckoutFormInitializeRequest
            {
                ConversationId = conversionId,
                Locale = Locale.TR.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                Price = price.ToString("0.00", CultureInfo.InvariantCulture),
                PaidPrice = paidPrice.ToString("0.00", CultureInfo.InvariantCulture),
                BasketId = orderNumber,
                CallbackUrl = _iyzicoConfig.CallbackUrl,
                Currency = Currency.TRY.ToString(),
                Buyer = buyer,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                BasketItems = newBasketItems,
                EnabledInstallments = enabledInstallments,
            };

            var payment = CheckoutFormInitialize.Create(paymentRequest, _option);
            return payment;
        }

        public CheckoutForm RequestForm(string token)
        {
            var request = new RetrieveCheckoutFormRequest {Token = token};

            var checkoutForm = CheckoutForm.Retrieve(request, _option);
            return checkoutForm;
        }
    }
}