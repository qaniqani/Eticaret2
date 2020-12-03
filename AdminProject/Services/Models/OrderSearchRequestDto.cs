using System;
using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class OrderSearchRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string OrderNr { get; set; }
        public int Take { get; set; } = 20;
        public int Skip { get; set; } = 1;
        public PayTypes PayType { get; set; }
        public OrderTypes OrderType { get; set; }
        //public ShipmentTypes ShipmentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}