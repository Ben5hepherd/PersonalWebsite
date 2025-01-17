using Microsoft.EntityFrameworkCore;
using PersonalWebsite.Api.Entities;

namespace PersonalWebsite.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
