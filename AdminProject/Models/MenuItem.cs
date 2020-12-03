using System.Collections.Generic;

namespace AdminProject.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public IEnumerable<MenuItem> ParentItem { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public int Number { get; set; }
        public StatusTypes Status { get; set; }
    }
}