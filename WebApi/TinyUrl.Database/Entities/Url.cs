using System.ComponentModel.DataAnnotations;

namespace TinyUrl.Database.Entities
{
    public class Url
    {
        [Key]
        [MinLength(5)]
        [MaxLength(30)]
        public string Alias { get; set; }
        public string ShortUrl { get; set; }
        public string OriginalUrl { get; set; }
        public DateTime UrlCreated { get; set; }
        public bool IsEdited { get; set; } 

        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
