using System.Collections.Generic;
using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class UserDetailView
    {
        public User User { get; set; }
        public IEnumerable<Address> Address { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<ProductCommentList> Comments { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
    }
}