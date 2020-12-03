using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using AdminProject.Infrastructure.Models;
using AdminProject.Services.Interface;

namespace AdminProject.Services
{
    public class RssService : IRssService
    {
        public SyndicationFeed GetFeedList(List<Product> products, string domain)
        {
            var feed = new SyndicationFeed
            {
                Id = "jkdsfhkjhjwelr",
                Title =
                    SyndicationContent.CreatePlaintextContent(
                        "Hediyelik Tesbih, Yüzük, Alyans, Kolye, Bileklik | Payidar.com.tr"),
                Description =
                    SyndicationContent.CreatePlaintextContent(
                        "Hediyelik tesbih, yüzük, alyans, kolye, bileklik, takı, özel günler, kol saati, kombinler aradığınız her çeşit aksesuar bizde mevcut."),
                LastUpdatedTime = DateTime.Now,
                ImageUrl = new Uri("https://payidar.com.tr/theme/images/content/logo.png"),
                Copyright = SyndicationContent.CreatePlaintextContent($"© {DateTime.Now.Year} Payidar.com.tr"),
                //Language = "tr-TR",
                //BaseUri = new Uri(domain),
                Items = products.Select(a =>
                {
                    var item = new SyndicationItem
                    {
                        Title = new TextSyndicationContent(a.Name, TextSyndicationContentKind.Plaintext),
                        BaseUri = new Uri(domain),
                        Id = a.Id.ToString(),
                        PublishDate = a.CreateDate,
                        LastUpdatedTime = a.UpdateDate > new DateTime(2017, 1, 1) ? a.UpdateDate : a.CreateDate,
                        Summary = new TextSyndicationContent(a.SeoDescription),
                    };

                    item.Links.Add(new SyndicationLink { BaseUri = new Uri(domain), Uri = new Uri($"{domain}product/{a.Url}/detail")});
                    return item;
                })
            };

            return feed;
        }
    }
}