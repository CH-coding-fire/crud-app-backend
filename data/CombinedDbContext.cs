using fourthAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace fourthAPI.data
{
    public class CombinedDbContext : DbContext  // Added missing class declaration and base class
    {
        public CombinedDbContext(DbContextOptions<CombinedDbContext> options)
            : base(options)
        {
        }
        public DbSet<Team> Teams { get; set; }  // Typically DbSets are named in plural
        public DbSet<TodoGroup> TodoGroups { get; set; }
        public DbSet<TodoItem> TodoItems { get; set; }  // This should probably be plural too for convention
        public DbSet<User> Users { get; set; }  // Renamed to plural for consistency
    }
}
