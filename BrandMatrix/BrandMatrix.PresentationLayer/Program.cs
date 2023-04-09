using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.DataAccessLayer.Repositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ISqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DomainSettings");
    var logger = sp.GetRequiredService<ILogger<SqlConnectionService>>();
    return new SqlConnectionService(connectionString, logger);
});

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller}/{action}/{id?}",
    defaults: new { area = "Admin", controller = "Home", action = "Dashboard" }
);

app.Run();
