using System;
using System.Collections.Generic;

namespace AdminProject.Models
{
    public class UserDetailViewDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string TcNr { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string NewPassword2 { get; set; }
        public DateTime LastLoginDate { get; set; }
        public UserTypes Status { get; set; }
        public List<UserListItem> AddressList { get; set; }
        public List<UserListItem> InvoiceList { get; set; }
        public List<UserListItem> CommentList { get; set; }
        public List<UserListItem> OrderList { get; set; }
    }

    public class UserListItem
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Detail { get; set; }
        public DateTime DateTime { get; set; }
    }
}