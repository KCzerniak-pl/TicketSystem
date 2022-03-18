using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class CategoriesDbContext : DbContext
    {
        // Hide default constructor.
        protected CategoriesDbContext() { }

        public CategoriesDbContext(DbContextOptions<CategoriesDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<Category> Categories { get; set; } = default!;
    }
}
