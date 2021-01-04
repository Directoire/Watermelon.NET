using Microsoft.EntityFrameworkCore;
using Watermelon.NET.Data.Models;

namespace Watermelon.NET.Data.Context
{
    public class WatermelonContext : DbContext
    {
        public WatermelonContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<User> Users { get; set; }
    }
}