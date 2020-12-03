using System.Collections.Generic;
using System.ServiceModel.Syndication;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Interface
{
    public interface IRssService
    {
        SyndicationFeed GetFeedList(List<Product> products, string domain);
    }
}