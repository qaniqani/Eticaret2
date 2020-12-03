using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class File
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadUserId { get; set; }
        public StatusTypes Status { get; set; }
        public int Hit { get; set; }
    }
}