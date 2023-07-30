namespace TinyUrl.API.Models.Responce
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
        public DateTime TokenExpiration { get; set; }
        public Guid RefreshToken { get; set; }
    }
}
