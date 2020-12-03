using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Cargo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public decimal Price { get; set; }
        public bool IsPayDoor { get; set; }
        public bool DefaultCargo { get; set; }
        public StatusTypes Status { get; set; }
    }
}