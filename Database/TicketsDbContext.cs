using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class TicketsDbContext : DbContext
    {
        // Hide default constructor.
        protected TicketsDbContext() { }

        public TicketsDbContext(DbContextOptions<TicketsDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<Ticket> Tickets { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Owner)
                .WithMany(b => b.OwnerTickets)
                .HasForeignKey(s => s.OwnerID)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Technician)
                .WithMany(b => b.TechnicianTickets)
                .HasForeignKey(s => s.TechnicianID)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Status)
                .WithMany(b => b.Tickets)
                .HasForeignKey(s => s.StatusID)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Category)
                .WithMany(b => b.Tickets)
                .HasForeignKey(s => s.CategoryID)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
