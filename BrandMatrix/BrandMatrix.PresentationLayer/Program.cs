using BrandMatrix.BusinessLogicLayer.IRepositories;
using BrandMatrix.DataAccessLayer.Repositories;
using BrandMatrix.Domain.Interfaces;
using BrandMatrix.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


#region Build In Services

builder.Services.AddControllersWithViews();

#endregion

#region Data Access Layer Services 

builder.Services.AddScoped<ISqlConnection>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DomainSettings");
    var logger = sp.GetRequiredService<ILogger<SqlConnectionService>>();
    return new SqlConnectionService(connectionString, logger);
});

#endregion

#region Business Logic Layer Services

builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

#endregion Access Layer Services

#region 3rd Party Services

builder.Services.AddSession();

#endregion

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
    name: "User",
    pattern: "{area:exists}/{controller}/{action}/{id?}",
    defaults: new { area = "User", controller = "Accounts", action = "SigninUser" }
);

app.Run();
