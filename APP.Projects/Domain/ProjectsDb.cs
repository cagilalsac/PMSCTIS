using Microsoft.EntityFrameworkCore;

namespace APP.Projects.Domain
{
    /// <summary>
    /// Represents the database context for the projects, inheriting from <see cref="DbContext"/>.
    /// </summary>
    public class ProjectsDb : DbContext
    {
        /// <summary>
        /// Gets or sets the Tags DbSet, which represents the collection of all Tag entities in the context.
        /// </summary>
        public DbSet<Tag> Tags { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectsDb"/> class with the specified options.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public ProjectsDb(DbContextOptions options) : base(options)
        {
        }

        // The connection string should be defined in appsettings.json of the API Project.
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder.UseSqlServer("server=(localdb)\\mssqllocaldb;database=PMSCTISProjectsDB;trusted_connection=true;"));
        //}
    }
}
