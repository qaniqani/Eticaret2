using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExchangeType { get; set; } //tl $ €
        public string Branch { get; set; } //sube
        public string BranchCode { get; set; }
        public string AccountNo { get; set; }
        public string Iban { get; set; }
        public int SequenceNr { get; set; }
        public StatusTypes Status { get; set; }
    }
}