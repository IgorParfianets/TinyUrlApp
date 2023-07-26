namespace TinyUrl.Database.Entities
{
    public class Link
    {
        public Guid Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime UrlCreated { get; set; }
        public bool IsEdited { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
