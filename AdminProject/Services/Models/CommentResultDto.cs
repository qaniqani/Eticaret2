using System;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class CommentResultDto
    {
        public int CommentId { get; set; }
        public int UserId { get; set; } 
        public int ProductId { get; set; } 
        public string CommentDetail { get; set; }
        public string ProductName { get; set; } 
        public string ProductUrl { get; set; } 
        public string UserName { get; set; } 
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public DateTime CreateDate { get; set; }
        public CommentTypes Status { get; set; }
    }
}