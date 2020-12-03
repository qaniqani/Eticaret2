using System.Web.Mvc;

namespace AdminProject.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                new[] { "AdminProject.Areas.Admin.Controllers" }
            );

            //context.MapRoute(
            //    "Admin_Page",
            //    "Admin/{controller}/{action}",
            //    new[] { "AdminProject.Areas.Admin.Controllers" },
            //    new { controller = "Login", action = "Index" }
            //);

            //context.MapRoute(
            //    "Admin_default",
            //    "Admin/{controller}/{action}/{id}",
            //    new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}