using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticket System API", Version = "v1" });
});

// Context for database connection (required references to the library "Database").
string connectonString = builder.Configuration.GetConnectionString("TicketSystemDatabase");
builder.Services.AddDbContext<Database.TicketsDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.MessagesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.CategoriesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.StatusesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.UsersDbContext>(opt => opt.UseSqlServer(connectonString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        // Endpoint - set address and name JSON document.
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket System API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Database initialize.
DatabaseInitialize(app);

app.Run();

void DatabaseInitialize(IApplicationBuilder app)
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
                // Add initialize data.
                var Names = builder.Configuration.GetSection("DatabaseInitialize:Statues").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).ToList();
                foreach (var name in Names)
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
                // Add initialize data.
                var user = builder.Configuration.GetSection("DatabaseInitialize:User").AsEnumerable().Where(p => p.Value != null).Select(p => p.Value).Reverse().ToList();
                var roleID = new Guid();
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