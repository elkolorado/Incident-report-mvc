using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ErrorReports.Areas.Identity.Data;
using ErrorReports.Models;
using Microsoft.AspNetCore.Authorization;
using ErrorReports.Authorization;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDBContextConnection") ?? throw new InvalidOperationException("Connection string 'AppDBContextConnection' not found.");

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDBContext>();



// Add services to the container.
builder.Services.AddControllersWithViews();

// Authorization handlers.
builder.Services.AddScoped<IAuthorizationHandler,
                      IncidentIsOwnerAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                      IncidentIsAdminAuthorizationHandler>();

builder.Services.AddSingleton<IAuthorizationHandler,
                      IncidentIsManagerAuthorizationHandler>();

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
app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ErrorReport}/{action=Index}/{id?}");


//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var context = services.GetRequiredService<AppDBContext>();
//    context.Database.Migrate();
//    // requires using Microsoft.Extensions.Configuration;
//    // Set password with the Secret Manager tool.
//    // dotnet user-secrets set SeedUserPW <pw>

//    var testUserPw = builder.Configuration.GetValue<string>("SeedUserPW");

//    await SeedData.Initialize(services, testUserPw);
//}


app.Run();
