using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class UsersDbContext : DbContext
    {
        // Hide default constructor.
        protected UsersDbContext() { }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<User> Users { get; set; } = default!;
    }
}
