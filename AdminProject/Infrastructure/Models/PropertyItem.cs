using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class PropertyItem
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}