using ErrorReports.Areas.Identity.Data;
using ErrorReports.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ErrorReports.Authorization
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new AppDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDBContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com", "Admin Name", "Admin Surname");
                await EnsureRole(serviceProvider, adminID, Constants.IncidentAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com", "Manager Name", "Manager Surname");
                await EnsureRole(serviceProvider, managerID, Constants.IncidentManagersRole);

                SeedDB(context, adminID);
            }
        }
        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName, string FirstName, string LastName)
        {
            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new AppUser
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            IdentityResult IR;
            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<AppUser>>();

            //if (userManager == null)
            //{
            //    throw new Exception("userManager is null");
            //}

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }


        public static void SeedDB(AppDBContext context, string adminID)
        {
            if (context.Incidents.Any())
            {
                return;   // DB has been seeded
            }

            context.Incidents.AddRange(
                new ErrorReport
                {
                    Id = 1,
                    Title = "Błąd w aplikacji",
                    Description = "Aplikacja zawiesza się.",
                    DateReported = DateTime.Now.AddHours(-3),
                    ReporterName = adminID,
                    Status = ErrorStatus.Open,
                    Priority = ErrorPriority.High
                }
             );
            context.SaveChanges();
        }

    }
}