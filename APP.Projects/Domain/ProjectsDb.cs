using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Domain
{
    public class ProjectsDb : DbContext
    {
        public DbSet<Tag> Tags { get; set; }

        public ProjectsDb(DbContextOptions options) : base(options)
        {
        }

        // Connection string should be defined in appsettings.json of the API Project
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;database=PMSCTISProjectsDB;trusted_connection=true;"));
        //}
    }
}
