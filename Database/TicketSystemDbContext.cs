using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class TicketSystemDbContext : DbContext
    {
        // Hide default constructor.
        protected TicketSystemDbContext() { }

        public TicketSystemDbContext(DbContextOptions<TicketSystemDbContext> options) : base(options) { }

        // Properties DbSet for entity that is mapped on database table and view.
        // DbSet represents the entity set which can use for create, read, update and delete data in database.
        public virtual DbSet<Ticket>? Tickets { get; set; }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Status>? Statuses { get; set; }
        public virtual DbSet<Message>? Messages { get; set; }
        public virtual DbSet<Category>? Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Owner)
                .WithMany(b => b.OwnerTickets)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Technician)
                .WithMany(b => b.TechnicianTickets)
                .HasForeignKey(s => s.TechnicianId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Status)
                .WithMany(b => b.Tickets)
                .HasForeignKey(s => s.StatusId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(p => p.Category)
                .WithMany(b => b.Tickets)
                .HasForeignKey(s => s.CategoryId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<User>()
                .HasOne(p => p.Role)
                .WithMany(b => b.Users)
                .HasForeignKey(s => s.RoleId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Message>()
                .HasOne(p => p.Owner)
                .WithMany(b => b.Messages)
                .HasForeignKey(s => s.OwnerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<Message>()
                .HasOne(p => p.Ticket)
                .WithMany(b => b.Messages)
                .HasForeignKey(s => s.TicketId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
