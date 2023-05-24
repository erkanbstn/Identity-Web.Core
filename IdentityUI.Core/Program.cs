using IdentityUI.Core.ClaimProvider;
using IdentityUI.Core.Extensions;
using IdentityUI.Core.Repository.Models;
using IdentityUI.Core.Core.OptionsModel;
using IdentityUI.Core.Core.Permissions;
using IdentityUI.Core.Requirements;
using IdentityUI.Core.Repository.Seeds;
using IdentityUI.Core.Service.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Sql"), options =>
    {
        options.MigrationsAssembly("IdentityUI.Core.Repository");
    });
});
builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval = TimeSpan.FromMinutes(30);
});
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("ÝstanbulPolicy", policy =>
    {
        policy.RequireClaim("city", "Ýstanbul", "Ankara");
    });
    opt.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });
    opt.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThreshOldAge = 18 });
    });
    opt.AddPolicy("OrderPermissionReadOrDelete", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Read);
        policy.RequireClaim("permission", Permission.Order.Delete);
        policy.RequireClaim("permission", Permission.Stock.Delete);
    });

    opt.AddPolicy("Permission.Order.Read", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Read);
    });
    opt.AddPolicy("Permission.Order.Delete", policy =>
    {
        policy.RequireClaim("permission", Permission.Order.Delete);
    });
    opt.AddPolicy("Permission.Stock.Delete", policy =>
    {
        policy.RequireClaim("permission", Permission.Stock.Delete);
    });
});
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddIdentityWithExt();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookieBuilder = new CookieBuilder();
    cookieBuilder.Name = "IdentityCookie";
    opt.LoginPath = new PathString("/Auth/SignIn");
    opt.Cookie = cookieBuilder;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
    opt.LogoutPath = new PathString("/Auth/SignOut");
    opt.AccessDeniedPath = new PathString("/Auth/AccessDenied");
});
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
    await PermissionSeed.Seed(roleManager);
}

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=SignUp}/{id?}");

app.Run();
