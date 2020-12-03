using System;

namespace AdminProject.Infrastructure.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductUrl { get; set; }
        public DateTime DateTime { get; set; }
    }
}