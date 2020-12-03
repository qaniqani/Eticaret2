using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Net.Http.Formatting;
using System.Web.Optimization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Web.Routing;
using AdminProject.App_Start;

namespace AdminProject
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //Install-Package Ninject.Web.WebApi 
            //install-package Ninject.Web.WebApi.WebHost
            GlobalConfiguration.Configure(config =>
            {
                config.MapHttpAttributeRoutes();

                //accept-header bilgisini alir
                config.MessageHandlers.Add(new LanguageHandler());

                var jsonFormatter = new JsonMediaTypeFormatter();

                var jsonSerializerSettings = jsonFormatter.SerializerSettings;
                jsonSerializerSettings.Formatting = Formatting.Indented;
                jsonSerializerSettings.Converters.Add(new StringEnumConverter());
                jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                config.Formatters.Clear();
                config.Formatters.Add(jsonFormatter);
            });

            AreaRegistration.RegisterAllAreas();
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0;
        }
    }
}
