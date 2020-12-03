using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string Detail { get; set; }
        public DateTime DateTime { get; set; }
        public CommentTypes Status { get; set; }
    }
}