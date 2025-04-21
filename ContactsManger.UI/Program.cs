using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Repositories;
using Services;
using RepositroyContracts;
using Enities;

var builder = WebApplication.CreateBuilder(args);

// Enable console logging (for Render logs)
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add MVC support with Razor view runtime compilation
builder.Services.AddControllersWithViews();
             

// Register repositories
builder.Services.AddScoped<IPersonRepositroy, PersonRepositroy>();
builder.Services.AddScoped<IcountryRepository, CountryRepositroy>();

// Register services
builder.Services.AddScoped<ICountriesServices, CountriesServices>();
builder.Services.AddScoped<IPersonService, PersonServices>();

// Register DbContext with connection string
builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCON"));
});

var app = builder.Build();

// Use detailed error page in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Use exception handler in production
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Enable HTTPS redirection and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

// Default MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
