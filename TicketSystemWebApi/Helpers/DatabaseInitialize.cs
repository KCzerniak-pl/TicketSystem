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
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var ticketSystemDbContext = serviceScope.ServiceProvider.GetService<Database.TicketsDbContext>();
                var statuesDbContext = serviceScope.ServiceProvider.GetService<Database.StatusesDbContext>();
                var usersDbContext = serviceScope.ServiceProvider.GetService<Database.UsersDbContext>();

                // Check the database is connectable.
                if (ticketSystemDbContext?.GetService<IRelationalDatabaseCreator>().Exists() == false)
                {
                    // Database migration.
                    ticketSystemDbContext.Database.Migrate();

                    // Check the tables exists.
                    if (statuesDbContext?.Database.GetService<IRelationalDatabaseCreator>().HasTables() == true)
                    {
                        // Options for json serializer.
                        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };

                        // Add initialize data for "Statuses" table.
                        IEnumerable<string> names = builder.Configuration.GetSection("DatabaseInitialize:Statuses").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse();
                        foreach (string name in names)
                        {
                            statuesDbContext.Statuses.Add(
                                new Database.Entities.Status
                                {
                                    StatusID = new Guid(),
                                    Name = name
                                }
                            );
                        }

                        statuesDbContext.SaveChanges();
                    }

                    // Check the tables exists.
                    if (usersDbContext?.Database.GetService<IRelationalDatabaseCreator>().HasTables() == true)
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
                                    ShowAll = true
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
