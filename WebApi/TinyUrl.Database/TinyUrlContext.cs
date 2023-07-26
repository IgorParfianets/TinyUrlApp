using Microsoft.EntityFrameworkCore;

namespace TinyUrl.Database
{
    public class TinyUrlContext : DbContext
    {
        public TinyUrlContext(DbContextOptions<TinyUrlContext> options) : base(options)
        {
        }
    }
}