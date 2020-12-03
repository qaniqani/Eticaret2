using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class GalleryDetail
    {
        public int Id { get; set; }
        public int GalleryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreateDate { get; set; }
        public int SequenceNumber { get; set; }
        public StatusTypes Status { get; set; }
    }
}