namespace AdminProject.Models
{
    public class SortedMenu
    {
        public string Depth { get; set; }
        public int ItemId { get; set; }
        public string Left { get; set; }
        public int ParentId { get; set; }
        public string Right { get; set; }
    }
}