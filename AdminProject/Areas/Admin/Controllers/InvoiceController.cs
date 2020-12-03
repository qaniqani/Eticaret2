using System.Web.Mvc;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(RuntimeSettings setting, IInvoiceService invoiceService) : base(setting)
        {
            _invoiceService = invoiceService;
        }

        public ActionResult View(int id)
        {
            var invoice = _invoiceService.GetInvoice(id);
            return View(invoice);
        }
    }
}