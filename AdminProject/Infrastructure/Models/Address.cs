using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressSaveName { get; set; }
        public string NameSurname { get; set; }
        public string AddressDetail { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string TcNr { get; set; }
        public string AddressNote { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public StatusTypes Status { get; set; }
    }
}