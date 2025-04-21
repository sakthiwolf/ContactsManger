using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Repositories;
using Services;
using RepositroyContracts;
using Enities;
using Rotativa.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add MVC support
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
    // Connection string for production (update this as per your environment)
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCON"));
});

var app = builder.Build();

// Apply pending migrations at startup (ensure DB is up-to-date in production)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PersonsDbContext>();
    dbContext.Database.Migrate();  // Apply migrations on startup
}

// Enable detailed error page only in development
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    // Exception handling for production environment
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Rotativa setup for PDF generation (if used)
RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

// Middleware
app.UseStaticFiles();
app.UseRouting();

// Default routing for MVC
app.MapControllers();

app.Run();
