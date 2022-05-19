﻿// <auto-generated />
using System;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Database.Migrations.TicketsMigrations
{
    [DbContext(typeof(TicketsDbContext))]
    partial class TicketsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Database.Entities.Category", b =>
                {
                    b.Property<Guid>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Database.Entities.Message", b =>
                {
                    b.Property<Guid>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTimeCreated")
                        .HasColumnType("datetimeoffset(0)");

                    b.Property<string>("Information")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("TicketID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("MessageID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("TicketID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Database.Entities.Status", b =>
                {
                    b.Property<Guid>("StatusID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StatusID");

                    b.ToTable("Statuses");
                });

            modelBuilder.Entity("Database.Entities.Ticket", b =>
                {
                    b.Property<Guid>("TicketID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CategoryID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTimeCreated")
                        .HasColumnType("datetimeoffset(0)");

                    b.Property<DateTimeOffset>("DateTimeModified")
                        .HasColumnType("datetimeoffset(0)");

                    b.Property<int>("No")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("No"), 1L, 1);

                    b.Property<Guid>("OwnerID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("StatusID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TechnicianID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(75)");

                    b.HasKey("TicketID");

                    b.HasIndex("CategoryID");

                    b.HasIndex("OwnerID");

                    b.HasIndex("StatusID");

                    b.HasIndex("TechnicianID");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Database.Entities.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTimeCreated")
                        .HasColumnType("datetimeoffset(0)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Database.Entities.UserRole", b =>
                {
                    b.Property<Guid>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanAccepted")
                        .HasColumnType("bit");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("ShowAll")
                        .HasColumnType("bit");

                    b.Property<bool>("Technician")
                        .HasColumnType("bit");

                    b.HasKey("RoleID");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Database.Entities.Message", b =>
                {
                    b.HasOne("Database.Entities.User", "Owner")
                        .WithMany("Messages")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Database.Entities.Ticket", "Ticket")
                        .WithMany("Messages")
                        .HasForeignKey("TicketID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Database.Entities.Ticket", b =>
                {
                    b.HasOne("Database.Entities.Category", "Category")
                        .WithMany("Tickets")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Database.Entities.User", "Owner")
                        .WithMany("OwnerTickets")
                        .HasForeignKey("OwnerID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Database.Entities.Status", "Status")
                        .WithMany("Tickets")
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Database.Entities.User", "Technician")
                        .WithMany("TechnicianTickets")
                        .HasForeignKey("TechnicianID");

                    b.Navigation("Category");

                    b.Navigation("Owner");

                    b.Navigation("Status");

                    b.Navigation("Technician");
                });

            modelBuilder.Entity("Database.Entities.User", b =>
                {
                    b.HasOne("Database.Entities.UserRole", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Database.Entities.Category", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Database.Entities.Status", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("Database.Entities.Ticket", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Database.Entities.User", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("OwnerTickets");

                    b.Navigation("TechnicianTickets");
                });

            modelBuilder.Entity("Database.Entities.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
