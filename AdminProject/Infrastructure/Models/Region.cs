using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Region
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string CityName { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}