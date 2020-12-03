using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminProject.Infrastructure.Models
{
    public class Basket
    {
        [Key]
        public int Id { get; set; }
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public string ProductUrl { get; set; }
        public int CargoId { get; set; }
        [ForeignKey("CargoId")]
        public Cargo Cargo { get; set; }
        public int Unit { get; set; }
        public decimal Price { get; set; }
        public string IpAddress { get; set; }
        public string OtherDetail { get; set; } //json -> name - value (olculer icin kullanilabilir)
        public DateTime DateTime { get; set; }
    }
}