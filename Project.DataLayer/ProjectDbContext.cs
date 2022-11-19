using Microsoft.EntityFrameworkCore;
namespace Project.DataLayer
{
    public class ProjectDbContext:DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options):base(options)
        {

        }
        public DbSet<User> Tbl_Users { get; set; }
        public DbSet<Category> Tbl_Categories { get; set; }
    }
}
