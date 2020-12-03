using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class CategoryListDto
    {
        public int Id { get; set; }
        public string ChainName { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Picture { get; set; }
        public StatusTypes Status { get; set; }
    }
}