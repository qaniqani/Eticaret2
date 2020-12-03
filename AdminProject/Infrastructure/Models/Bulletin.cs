using System;

namespace AdminProject.Infrastructure.Models
{
    public class Bulletin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}