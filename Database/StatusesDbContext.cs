using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class StatusesDbContext : DbContext
    {
        // Hide default constructor.
        protected StatusesDbContext() { }

        public StatusesDbContext(DbContextOptions<StatusesDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<Status> Statuses { get; set; } = default!;
    }
}
