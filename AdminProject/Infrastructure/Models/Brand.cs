using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}