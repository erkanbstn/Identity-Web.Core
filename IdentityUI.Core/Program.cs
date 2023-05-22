using IdentityUI.Core.Extensions;
using IdentityUI.Core.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Sql"));
});
builder.Services.AddIdentityWithExt();
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.User.AllowedUserNameCharacters = "abcdefghijklmnoprstuvwxyz1234567890_";

    opt.Password.RequiredLength = 6;
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireDigit = false;

}).AddEntityFrameworkStores<AppDbContext>();
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


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignUp}/{id?}");

app.Run();
