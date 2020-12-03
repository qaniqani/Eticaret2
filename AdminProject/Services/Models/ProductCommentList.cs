using AdminProject.Infrastructure.Models;

namespace AdminProject.Services.Models
{
    public class ProductCommentList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Comment Comment { get; set; }
    }
}