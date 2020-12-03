using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IInvoiceService
    {
        void Freeze(int id);
        void Add(Invoice invoice);
        void Edit(int id, Invoice invoiceRequest);
        Invoice GetInvoice(int id);
        Invoice GetInvoice(int id, int userId);
        List<Invoice> AllInvoiceList(int userId);
        List<Invoice> ActiveInvoiceList(int userId);
        List<Invoice> GetUserInoive(int userId);
        int AddReturnId(Invoice invoice);
    }
}