using System;

namespace AdminProject.Models
{
    public class RssItemModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
    }
}