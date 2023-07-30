namespace TinyUrl.Core.DataTransferObjects
{
    public class UrlDto
    {
        public string Alias { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortUrl { get; set; }
        public DateTime UrlCreated { get; set; }
    }
}
