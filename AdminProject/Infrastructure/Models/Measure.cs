using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Measure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public StatusTypes Status { get; set; }
    }
}