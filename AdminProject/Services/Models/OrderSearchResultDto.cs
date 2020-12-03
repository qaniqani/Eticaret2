using System;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class OrderSearchResultDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int CargoId { get; set; }
        public int InvoiceId { get; set; }
        public int AddressId { get; set; }
        public string OrderNr { get; set; }
        public OrderTypes OrderType { get; set; }
        public PayTypes PayType { get; set; }
        //public ShipmentTypes ShipmentType { get; set; }
        public string CargoNr { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string OrderNote { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdateUserId { get; set; } = 0;
    }
}