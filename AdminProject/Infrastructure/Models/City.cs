using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class City
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string PlateNo { get; set; }
        public string PhoneCode { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}