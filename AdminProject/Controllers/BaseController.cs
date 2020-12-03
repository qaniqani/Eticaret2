using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Attributes;
using AdminProject.Models;

namespace AdminProject.Controllers
{
    public class BaseController : Controller
    {
        [CookieCheck]
        public string GetIpAddress()
        {
            return !string.IsNullOrEmpty(Request.UserHostAddress) ? Request.UserHostAddress : Request.ServerVariables["REMOTE_ADDR"];
        }

        public string GetSessionId()
        {
            return Session.SessionID;
        }

        public string GetErrorMessage(ICollection<ModelState> state)
        {
            var errors = string.Join(", ", state.SelectMany(a => a.Errors.Select(e => e.ErrorMessage)));
            return errors;
        }

        public string OrderNumber()
        {
            return Session["OrderNumber"].ToString();
        }

        public void OrderNumber(string orderNumber)
        {
            Session["OrderNumber"] = orderNumber;
        }

        public PreSaveOrderInfo BasketInfo()
        {
            return Session["BasketInfo"] as PreSaveOrderInfo;
        }

        public void BasketInfo(PreSaveOrderInfo info)
        {
            Session["BasketInfo"] = info;
        }

        public void ErrorMessage(string message)
        {
            TempData["Error"] = message;
        }

        public void SuccessMessage(string message)
        {
            TempData["Success"] = message;
        }
    }
}