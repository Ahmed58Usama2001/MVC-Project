using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Demo.PL.Extensions;
using Demo.PL.Helpers;
using Demo.PL.Setting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Register built-in MVC services to the container

builder.Services.AddDbContext<AppDBContext>(options =>
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json")
        .Build();

    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Demo.DAL"));
});

//ApplicationServicesExtensions.AddAplicationServices(builder.Services); //Static methid
builder.Services.AddAplicationServices();  //Extension Method

builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));

//builder.Services.AddScoped<UserManager<ApplicationUser>>();
//builder.Services.AddScoped<SignInManager<ApplicationUser>>();
//builder.Services.AddScoped<RoleManager<IdentityRole>>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(config =>
{
    config.Password.RequiredUniqueChars = 2;
    config.Password.RequireDigit = true;
    config.Password.RequireNonAlphanumeric = true;
    config.Password.RequiredLength = 5;
    config.Password.RequireUppercase=true;
    config.Password.RequireLowercase=true;
    config.User.RequireUniqueEmail = true;
    config.Lockout.MaxFailedAccessAttempts = 3;
    config.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    config.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<AppDBContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
	config.LoginPath = "/Account/SignIn";
    config.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)//Added by default
//	.AddCookie("Another", config =>
//    {
//        config.LoginPath = "/Account/SignIn";
//        config.AccessDeniedPath= "/Home/Index";
//    });

builder.Services.Configure<MailSetting>(builder.Configuration.GetSection("MailSetting"));

builder.Services.AddTransient<IMailSetting,EmailSettings>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
