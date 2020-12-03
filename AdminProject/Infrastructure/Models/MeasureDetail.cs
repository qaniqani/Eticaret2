using AdminProject.Models;

namespace AdminProject.Infrastructure.Models
{
    public class MeasureDetail
    {
        public int Id { get; set; }
        public int MeasureId { get; set; }
        public string Size { get; set; }
        public StatusTypes Status { get; set; }
    }
}