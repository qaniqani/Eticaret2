using System;
using System.Linq;
using System.Collections.Generic;
using AdminProject.Infrastructure;
using AdminProject.Services.Interface;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public InvoiceService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void Add(Invoice invoice)
        {
            var db = _dbFactory();
            db.Invoices.Add(invoice);
            db.SaveChanges();
        }

        public int AddReturnId(Invoice invoice)
        {
            var db = _dbFactory();
            db.Invoices.Add(invoice);
            db.SaveChanges();

            return invoice.Id;
        }

        public void Edit(int id, Invoice invoiceRequest)
        {
            var db = _dbFactory();
            var invoice = db.Invoices.FirstOrDefault(a => a.Id == id);
            invoice.Address = invoiceRequest.Address;
            invoice.City = invoiceRequest.City;
            invoice.Country = invoiceRequest.Country;
            invoice.Gsm = invoiceRequest.Gsm;
            invoice.InvoiceSaveName = invoiceRequest.InvoiceSaveName;
            invoice.InvoiceType = invoiceRequest.InvoiceType;
            invoice.IsEInvoice = invoiceRequest.IsEInvoice;
            invoice.NameSurname = invoiceRequest.NameSurname;
            invoice.Phone = invoiceRequest.Phone;
            invoice.Region = invoiceRequest.Region;
            invoice.TaxNr = invoiceRequest.TaxNr;
            invoice.TaxOffice = invoiceRequest.TaxOffice;
            invoice.UserId = invoiceRequest.UserId;
            db.SaveChanges();
        }

        public void Freeze(int id)
        {
            var db = _dbFactory();
            var invoice = db.Invoices.FirstOrDefault(a => a.Id == id);
            invoice.Status = AdminProject.Models.StatusTypes.Freeze;
            db.SaveChanges();
        }

        public Invoice GetInvoice(int id)
        {
            var db = _dbFactory();
            var invoice = db.Invoices.FirstOrDefault(a => a.Id == id);
            return invoice;
        }

        public Invoice GetInvoice(int id, int userId)
        {
            var db = _dbFactory();
            var invoice = db.Invoices.FirstOrDefault(a => a.Id == id && a.UserId == userId);
            return invoice;
        }

        public List<Invoice> GetUserInoive(int userId)
        {
            var db = _dbFactory();
            var invoices = db.Invoices.Where(a => a.UserId == userId && a.Status != AdminProject.Models.StatusTypes.Freeze).OrderBy(a => a.InvoiceSaveName).ToList();
            return invoices;
        }

        public List<Invoice> AllInvoiceList(int userId)
        {
            var db = _dbFactory();
            var list = db.Invoices.Where(a => a.UserId == userId).OrderBy(a => a.InvoiceSaveName).ToList();
            return list;
        }

        public List<Invoice> ActiveInvoiceList(int userId)
        {
            var db = _dbFactory();
            var list = db.Invoices.Where(a => a.UserId == userId && a.Status == AdminProject.Models.StatusTypes.Active).ToList();
            return list;
        }
    }
}