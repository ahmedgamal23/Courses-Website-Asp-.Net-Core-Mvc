using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CoursesWebsite.Data;
using CoursesWebsite.Areas.Admin.Data;
using CoursesWebsite.Models;
using CoursesWebsite.Areas.InstructorsArea.Data;
using CoursesWebsite.Areas.User.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using CoursesWebsite.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

// Connection string
var connectionString = builder.Configuration.GetConnectionString("CoursesWebsiteDbContextConnection")
    ?? throw new InvalidOperationException("Connection string 'CoursesWebsiteDbContextConnection' not found.");

// Add DbContext and Identity services
builder.Services.AddDbContext<CoursesWebsiteDbContext>(options =>
    options.UseSqlServer(connectionString));

// Adding Identity with custom ApplicationUser
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
}).AddRoles<IdentityRole>()
.AddRoleManager<RoleManager<IdentityRole>>()
.AddEntityFrameworkStores<CoursesWebsiteDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, EmailSender>();

// Add Razor Pages and MVC services
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages(); // <-- Add this line to register Razor Pages

// Register your scoped services
builder.Services.AddScoped(typeof(IServiceData<>), typeof(UserService<>));
builder.Services.AddScoped<IServiceData<Category>, CategoryItem>();
builder.Services.AddScoped<ICoursesItems, CourseItem>();
builder.Services.AddScoped<IVideoService, VideoService>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.Initializer(services);
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Configure route for areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Configure default route for MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Map Razor Pages (ensure Razor Pages services are properly registered)
app.MapRazorPages(); // <-- This needs Razor Pages services, so ensure AddRazorPages() is called

app.Run();



