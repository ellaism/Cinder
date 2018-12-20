using EllaX.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EllaX.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Peer> Peers { get; set; }
    }
}
