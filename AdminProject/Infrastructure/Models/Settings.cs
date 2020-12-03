using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int LanguageId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string MailAddress { get; set; }
        public string MailPassword { get; set; }
        public string Smtp { get; set; }
        public string Port { get; set; }
        public DateTime CreateDate { get; set; }
        public StatusTypes Status { get; set; }
    }
}