using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using Iyzipay.Model;


namespace AdminProject.Services.Interface
{
    public interface IIyzipayService
    {
        CheckoutFormInitialize InitializeForm(User user, Infrastructure.Models.Address deliveryAddress,
            Invoice invoiceAddress, BasketListDto basketItems, string ipAddress, string orderNumber);

        CheckoutForm RequestForm(string token);
    }
}