using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AdminProject.Helpers
{
    public class RssResult : FileResult
    {
        private readonly SyndicationFeed _feed;

        public RssResult(SyndicationFeed feed) : base("application/xml")
        {
            _feed = feed;
        }

        public RssResult(string title, IEnumerable<SyndicationItem> feedItems) : base("application/xml")
        {
            _feed = new SyndicationFeed(title, title, HttpContext.Current.Request.Url) { Items = feedItems };
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            using (var writer = XmlWriter.Create(response.OutputStream))
            {
                _feed.GetRss20Formatter().WriteTo(writer);
            }
        }
    }

    public class RssActionResult : ActionResult
    {
        public SyndicationFeed Feed { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "application/rss+xml";
            var rssFormatter = new Rss20FeedFormatter(Feed);
            using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
            {
                rssFormatter.WriteTo(writer);
            }
        }
    }
}