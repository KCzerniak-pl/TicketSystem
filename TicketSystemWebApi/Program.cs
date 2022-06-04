using EmailService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TicketSystemWebApi.Helpers;
using TicketSystemWebApi.Helpers.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Ticket System API", Version = "v1" });

    // JWT - config for Swagger.
    x.AddSecurityDefinition("BearerAuth", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme.ToLowerInvariant(),
        In = ParameterLocation.Header,
        Name = "Authorization",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    // JWT - operation filter for Swagger.
    x.OperationFilter<AuthResponsesOperationFilter>();
});

// JWT - Get JWT configuration from "appsettings.json" and mapping this to object "JwtConfig".
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

// JWT - Validate token.
JwtAuthorizationExtension.AddJwtAuthorization(builder.Services);

// Context for database connection (required references to the library "Database").
string connectonString = builder.Configuration.GetConnectionString("TicketSystemDatabase");
builder.Services.AddDbContext<Database.TicketsDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.MessagesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.CategoriesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.StatusesDbContext>(opt => opt.UseSqlServer(connectonString));
builder.Services.AddDbContext<Database.UsersDbContext>(opt => opt.UseSqlServer(connectonString));

// Get email configuration from "appsettings.json" and mapping this to object "EmailConfiguration" (required references to the library "EmailService").
EmailConfiguration emailConfiguration = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
// Dependency injection (for email sending).
builder.Services.AddSingleton(emailConfiguration);
// Dependency injection - inverse of control (for email sending). Mapping interface to object.
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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