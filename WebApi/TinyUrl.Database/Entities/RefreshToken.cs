namespace TinyUrl.Database.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid Token { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
