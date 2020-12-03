using System;
using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime? BirthDate2 { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Gsm { get; set; }
        public string TcNr { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string ActivationCode { get; set; }
        public string BannedMessage { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public UserTypes Status { get; set; }
    }
}