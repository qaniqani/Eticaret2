using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public VideoTypes IsVideoLink { get; set; }
        public PictureTypes PictureType { get; set; }
        public string Name { get; set; }
        public string Detail1 { get; set; }
        public string Detail2 { get; set; }
        public string Detail3 { get; set; }
        public string VideoUrl { get; set; }
        public string VideoEmbedCode { get; set; }
        public string Picture1 { get; set; }
        public string Picture2 { get; set; }
        public string Picture3 { get; set; }
        public string Picture4 { get; set; }
        public string Picture5 { get; set; }
        public string Picture6 { get; set; }
        public string Picture7 { get; set; }
        public string Picture8 { get; set; }
        public string Picture9 { get; set; }
        public string Picture10 { get; set; }
        public int SequenceNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public StatusTypes Status { get; set; }
    }
}