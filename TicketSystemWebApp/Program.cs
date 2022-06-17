using Serilog;
using System.Text.RegularExpressions;
using TicketSystemWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Swap URL to lowercase.
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

// Dependency injection - inverse of control. Mapping interface to object. 
builder.Services.AddScoped<ITicketsService, TicketsService>();
builder.Services.AddScoped<IAccountService, AccountService>();

// Session configured.
builder.Services.AddSession(opt =>
{
    // Cookies name for session.
    opt.Cookie.Name = string.Format(".TicketSystem.Session.{0}", Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""));

    // Session lifetime. Every access to the session resets the timelife.
    opt.IdleTimeout = TimeSpan.FromMinutes(25);
});

// Anti-Forgery Token for forms.
builder.Services.AddAntiforgery(opt =>
{
    // Cookies name for session.
    opt.Cookie.Name = string.Format(".TicketSystem.Antiforgery.{0}", Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", ""));
});

// Get Serilog configuration from "appsettings.json".
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

// Session enabled.
app.UseSession();

app.UseRouting();

app.UseAuthorization();

// Add routing for main site with all tickets (/ticets/index).
app.MapControllerRoute(
    name: "ticets",
    pattern: "{controller=Ticets}/{action=Index}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
