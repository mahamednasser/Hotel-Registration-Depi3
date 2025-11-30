using Estra7a.DataAccess.Data;
using Estra7a.DataAccess.Repositories.IRepository;
using Estra7a.DataAccess.Repositories.Repository;
using Estra7a.Models.Models;
using Estra7a.Services;
using Estra7a.Services.ServiceContracts;
using Estra7a.Services.Services;
using Estra7a.Web.Hubs;
using Estra7a.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(3);
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
});

builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
})
.AddViewLocalization()
.AddDataAnnotationsLocalization();

builder.Services.AddRazorPages()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddSignalR();


builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var arCulture = new CultureInfo("ar");
    arCulture.NumberFormat = CultureInfo.InvariantCulture.NumberFormat; // استخدم النقطة بدلاً من الفاصلة

    var supportedCultures = new[]
    {
        new CultureInfo("en"),
        arCulture
    };

    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.AllowedForNewUsers = true;
});

builder.Services.Configure<stripesettings>(builder.Configuration.GetSection("Strip"));


//Register for Dependency Injection
builder.Services.AddScoped<IUnitOfWork , UnitOfWork>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IFavoriteService, FavoriteServices>();


builder.Services.AddHostedService<BackgroundBookingService>();




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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Strip:secretKey").Get<string>();

app.UseRouting();
app.UseRequestLocalization();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<RoomHub>("/roomHub");
app.MapHub<ChatHub>("/chathub");
await RoleSeeder.SeedRolesAsync(app);
await RoleSeeder.SeedSuperAdminAsync(app);
app.Run();
