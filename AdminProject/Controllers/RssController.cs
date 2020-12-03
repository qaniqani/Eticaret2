using System;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using AdminProject.Helpers;
using AdminProject.Services.Interface;

namespace AdminProject.Controllers
{
    public class RssController : Controller
    {
        private readonly IProductService _productService;
        private readonly IRssService _rssService;

        public RssController(IRssService rssService, IProductService productService)
        {
            _rssService = rssService;
            _productService = productService;
        }

        [HttpGet]
        [Route("feed/atom")]
        [OutputCache(Duration = 3600)]
        public RssResult Atom()
        {
            var helper = new UrlHelper(Request.RequestContext);
            var url = helper.Action("Index", "Default", new { }, Request.IsSecureConnection ? "https" : "http");

            var products = _productService.GetLast20Product();
            var feed = _rssService.GetFeedList(products, url);

            return new RssResult(feed);
        }

        [HttpGet]
        [Route("feed/rss")]
        [OutputCache(Duration = 3600)]
        public ContentResult Rss()
        {
            var helper = new UrlHelper(Request.RequestContext);
            var url = helper.Action("Index", "Default", new { }, Request.IsSecureConnection ? "https" : "http");

            var products = _productService.GetLast20Product();
            var rss = new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
              new XElement("rss",
                new XAttribute("version", "2.0"),
                  new XElement("channel",
                    new XElement("title", "Hediyelik Tesbih, Yüzük, Alyans, Kolye, Bileklik | Payidar.com.tr"),
                    new XElement("link", url),
                    new XElement("description", "Hediyelik tesbih, yüzük, alyans, kolye, bileklik, takı, özel günler, kol saati, kombinler aradığınız her çeşit aksesuar bizde mevcut."),
                    new XElement("copyright", $"© {DateTime.Now.Year} Payidar.com.tr"),
                  from item in products
                  select
                  new XElement("item",
                    new XElement("title", item.Name),
                    new XElement("description", item.SeoDescription),
                    new XElement("link", $"{url}product/{item.Url}/detail"),
                    new XElement("pubDate", item.CreateDate.ToString("R"))
                  )
                )
              )
            );

            return Content(rss.ToString(), "text/xml");
        }
    }
}