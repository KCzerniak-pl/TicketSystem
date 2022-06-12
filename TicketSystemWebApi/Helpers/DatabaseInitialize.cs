using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text.Json;

namespace TicketSystemWebApi.Helpers
{
    public class DatabaseInitialize
    {
        internal static void DatabaseInitializeData(IApplicationBuilder app, WebApplicationBuilder builder)
        {
            bool initialize = bool.Parse(builder.Configuration.GetSection("DatabaseInitialize:Initialize").Value);
            if (initialize)
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    Database.TicketSystemDbContext ticketSystemDbContext = serviceScope.ServiceProvider.GetService<Database.TicketSystemDbContext>()!;

                    // Check the database is connectable.
                    if (!ticketSystemDbContext.Database.CanConnect())
                    {
                        // Database migration.
                        ticketSystemDbContext.Database.Migrate();

                        // Check the table exists.
                        if (!ticketSystemDbContext.Statuses!.Any())
                        {
                            // Add initialize data for "Statuses" table.
                            IEnumerable<string> names = builder.Configuration.GetSection("DatabaseInitialize:Statuses").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();
                            foreach (string name in names)
                            {
                                ticketSystemDbContext.Statuses!.Add(
                                    new Database.Entities.Status
                                    {
                                        StatusId = new Guid(),
                                        Name = name
                                    }
                                );
                            }

                            ticketSystemDbContext.SaveChanges();
                        }

                        // Check the table exists.
                        if (!ticketSystemDbContext.Users!.Any())
                        {
                            // Add initialize data for "Users" and "UserRoles" tables.
                            IEnumerable<string> user = builder.Configuration.GetSection("DatabaseInitialize:User").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();
                            Guid roleId = new Guid();
                            ticketSystemDbContext.Users!.Add(
                                new Database.Entities.User
                                {
                                    UserId = new Guid(),
                                    Email = user.ElementAt(0),
                                    FirstName = "Admin",
                                    LastName = "Admin",
                                    PasswordHash = user.ElementAt(1),
                                    DateTimeCreated = DateTime.Now,
                                    Role = new Database.Entities.UserRole
                                    {
                                        RoleId = roleId,
                                        RoleName = "Admin",
                                        CanAccepted = true,
                                        ShowAll = true,
                                        Technician = true
                                    }
                                }
                            );

                            ticketSystemDbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
