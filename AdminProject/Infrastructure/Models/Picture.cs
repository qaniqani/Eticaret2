namespace AdminProject.Infrastructure.Models
{
    public class Picture
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string MinPicture { get; set; }
        public string BigPicture { get; set; }
        public bool IsShowcase { get; set; }
    }
}