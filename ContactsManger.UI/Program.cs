using Microsoft.EntityFrameworkCore;

using ServiceContracts;
using Repositories;
using Services;
using RepositroyContracts;
using Enities;


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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBCON"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Rotativa setup for PDF generation (if used)
Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");

// Middleware
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
