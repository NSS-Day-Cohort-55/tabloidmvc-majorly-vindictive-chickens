namespace TabloidMVC.Models
{
    public class PostImage
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public int PostId { get; set; }
    }
}
