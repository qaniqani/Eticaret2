using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string InvoiceSaveName { get; set; }
        public InoviceTypes InvoiceType { get; set; }
        public string NameSurname { get; set; } //Corporate -> Title
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string TaxNr { get; set; }
        public string TaxOffice { get; set; }
        public bool IsEInvoice { get; set; }
        public StatusTypes Status { get; set; }
    }
}