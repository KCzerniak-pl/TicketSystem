using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketSystemWebApi.Helpers;

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

app.UseAuthorization();

app.MapControllers();

// Database initialize.
DatabaseInitialize.DatabaseInitializeData(app, builder);

app.Run();