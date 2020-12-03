using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Infrastructure;

namespace AdminProject.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
            SessionCheck();
        }

        private void SessionCheck()
        {
            if (Utility.SessionCheck() == null)
                Redirect("/Login");
        }

        public void SetPageHeader(string pageSub, string pageSub1)
        {
            TempData["PageSub"] = pageSub;
            TempData["PageSub1"] = pageSub1;
        }

        public void Added()
        {
            TempData["Success"] = "Added successfully.";
        }

        public void Updated()
        {
            TempData["Success"] = "Updated successfully.";
        }

        public void Deleted()
        {
            TempData["Warning"] = "Deleted successfully.";
        }

        public void Warning()
        {
            TempData["Warning"] = "No related records.";
        }
    }
}