using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class MessagesDbContext : DbContext
    {
        // Hide default constructor.
        protected MessagesDbContext() { }

        public MessagesDbContext(DbContextOptions<MessagesDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<Message> Messages { get; set; } = default!;
    }
}
