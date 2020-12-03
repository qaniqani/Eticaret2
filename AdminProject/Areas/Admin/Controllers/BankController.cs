using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure.Models;
using AdminProject.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Areas.Admin.Controllers
{
    public class BankController : BaseController
    {
        private readonly IBankService _bankService;

        public BankController(IBankService bankService, RuntimeSettings setting) : base(setting)
        {
            _bankService = bankService;
        }

        [HttpGet]
        public ActionResult Add()
        {
            SetPageHeader("Bank", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            return View(new Bank {SequenceNr = 9999});
        }

        [HttpPost]
        public ActionResult Add(Bank request)
        {
            SetPageHeader("Bank", "Add");

            ViewBag.StatusList = DropdownTypes.GetStatus(StatusTypes.Active);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(request.AccountNo))
                ModelState.AddModelError("AccountNo", "Account No is required.");

            if (string.IsNullOrEmpty(request.Branch))
                ModelState.AddModelError("Branch", "Branch is required.");

            if (string.IsNullOrEmpty(request.BranchCode))
                ModelState.AddModelError("BranchCode", "Branch Code is required.");

            if (string.IsNullOrEmpty(request.ExchangeType))
                ModelState.AddModelError("ExchangeType", "Exchange Type is required.");

            if (string.IsNullOrEmpty(request.Iban))
                ModelState.AddModelError("Iban", "Iban is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            _bankService.Add(request);

            Added();

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SetPageHeader("Bank", "Edit");

            var bank = _bankService.GetBank(id);

            ViewBag.StatusList = DropdownTypes.GetStatus(bank.Status);

            return View(bank);
        }

        [HttpPost]
        public ActionResult Edit(Bank request)
        {
            SetPageHeader("Bank", "Edit");

            ViewBag.StatusList = DropdownTypes.GetStatus(request.Status);

            if (string.IsNullOrEmpty(request.Name))
                ModelState.AddModelError("Name", "Name is required.");

            if (string.IsNullOrEmpty(request.AccountNo))
                ModelState.AddModelError("AccountNo", "Account No is required.");

            if (string.IsNullOrEmpty(request.Branch))
                ModelState.AddModelError("Branch", "Branch is required.");

            if (string.IsNullOrEmpty(request.BranchCode))
                ModelState.AddModelError("BranchCode", "Branch Code is required.");

            if (string.IsNullOrEmpty(request.ExchangeType))
                ModelState.AddModelError("ExchangeType", "Exchange Type is required.");

            if (string.IsNullOrEmpty(request.Iban))
                ModelState.AddModelError("Iban", "Iban is required.");

            if (request.SequenceNr < 1)
                request.SequenceNr = 9999;

            if (!ModelState.IsValid)
                return View(request);

            _bankService.Edit(request.Id, request);

            Updated();

            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult List()
        {
            SetPageHeader("Bank", "List");

            var banks = _bankService.GetActiveBankAllList();

            return View(banks);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            _bankService.Delete(id);

            Deleted();

            return RedirectToAction("List");
        }
    }
}