using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TwoLetterCode { get; set; }
        public string ThreeLetterCode { get; set; }
        public string PhoneCode { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}