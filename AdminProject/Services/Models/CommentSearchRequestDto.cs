using AdminProject.Models;

namespace AdminProject.Services.Models
{
    public class CommentSearchRequestDto
    {
        public string ProductName { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string Email { get; set; }
        public CommentTypes Status { get; set; } = CommentTypes.New;
    }
}