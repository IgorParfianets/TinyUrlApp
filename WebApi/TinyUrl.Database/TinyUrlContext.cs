using Microsoft.EntityFrameworkCore;
using TinyUrl.Database.Entities;

namespace TinyUrl.Database
{
    public class TinyUrlContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Url> Urls { get; set; }  

        public TinyUrlContext(DbContextOptions<TinyUrlContext> options) : base(options)
        {
        }
    }
}