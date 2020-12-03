using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int LanguageId { get; set; }
        public CategoryTypes CategoryType { get; set; }
        public string LanguageTag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int ModifiedUser { get; set; }
        public StatusTypes Status { get; set; }
        public int Hit { get; set; }
        public int SingleHit { get; set; }
    }
}