using IdentityServer.API;
using IdentityServer.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string? assembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddDbContext<IdentityDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), opt => opt.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<IdentityUser>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = c =>
            c.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), opt => opt.MigrationsAssembly(assembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = c =>
            c.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), opt => opt.MigrationsAssembly(assembly));
    })
    .AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.InitializeData(app.Configuration);
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

app.Run();