using Microsoft.EntityFrameworkCore;
using ApocSurviveHub.API.Models;

namespace ApocSurviveHub.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Survivor> Survivors { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Coordinates> Coordinates { get; set; }
        public DbSet<Horde> Hordes { get; set; }
        public string DbPath { get; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Combine(path, "ApocSurviveHub.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
    }

}