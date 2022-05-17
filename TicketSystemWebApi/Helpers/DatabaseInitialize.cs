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
                    Database.TicketsDbContext ticketsDbContext = serviceScope.ServiceProvider.GetService<Database.TicketsDbContext>()!;
                    Database.UsersDbContext usersDbContext = serviceScope.ServiceProvider.GetService<Database.UsersDbContext>()!;
                    Database.StatusesDbContext statusesDbContext = serviceScope.ServiceProvider.GetService<Database.StatusesDbContext>()!;

                    // Check the database is connectable.
                    if (!ticketsDbContext.Database.CanConnect())
                    {
                        // Database migration.
                        ticketsDbContext.Database.Migrate();

                        // Check the table exists.
                        if (!statusesDbContext.Statuses.Any())
                        {
                            // Add initialize data for "Statuses" table.
                            IEnumerable<string> names = builder.Configuration.GetSection("DatabaseInitialize:Statuses").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();
                            foreach (string name in names)
                            {
                                statusesDbContext.Statuses.Add(
                                    new Database.Entities.Status
                                    {
                                        StatusID = new Guid(),
                                        Name = name
                                    }
                                );
                            }

                            statusesDbContext.SaveChanges();
                        }

                        // Check the table exists.
                        if (!usersDbContext.Users.Any())
                        {
                            // Add initialize data for "Users" and "UserRoles" tables.
                            IEnumerable<string> user = builder.Configuration.GetSection("DatabaseInitialize:User").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();
                            Guid roleID = new Guid();
                            usersDbContext.Users.Add(
                                new Database.Entities.User
                                {
                                    UserID = new Guid(),
                                    Email = user.ElementAt(0),
                                    FirstName = "Admin",
                                    LastName = "Admin",
                                    PasswordHash = user.ElementAt(1),
                                    DateTimeCreated = DateTime.Now,
                                    RoleID = roleID,
                                    Role = new Database.Entities.UserRole
                                    {
                                        RoleID = roleID,
                                        RoleName = "Admin",
                                        CanAccepted = true,
                                        ShowAll = true,
                                        Technician = true
                                    }
                                }
                            );

                            usersDbContext.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
