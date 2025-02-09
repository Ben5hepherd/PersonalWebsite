using Microsoft.EntityFrameworkCore;
using PersonalWebsiteBFF.Domain.Entities;

namespace PersonalWebsiteBFF.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public virtual DbSet<User> Users { get; set; }
    }
}
