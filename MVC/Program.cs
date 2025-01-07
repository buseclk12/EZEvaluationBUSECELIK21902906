using BLL.DAL;
using BLL.Models;
using BLL.Services;
using BLL.Services.Bases;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.GetSection(nameof(AppSettings)).Bind(new AppSettings());

builder.Services.AddDbContext<Db>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddScoped<IService<User, UserModel>, UserService>();
builder.Services.AddScoped<IService<Evaluated, EvaluatedModel>, EvaluatedService>();
builder.Services.AddScoped<IService<Evaluation, EvaluationModel>, EvaluationService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(config =>
    {
        config.LoginPath = "/Users/Login";
        config.AccessDeniedPath = "/Users/Login";
        config.ExpireTimeSpan = TimeSpan.FromMinutes(30); 
        config.SlidingExpiration = true;
    });
builder.Services.AddSession(config =>
{
    config.IdleTimeout = TimeSpan.FromMinutes(30);
});

var app = builder.Build();

// PostgreSQL Configuration:
app.UseAuthentication();
app.UseSession();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
