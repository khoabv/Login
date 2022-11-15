using Login.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Reflection;
using Login.DataContext;

namespace Login.Utils
{
    public class DataSeeder
    {
        public static async Task Initialize(ApplicationContext context)
        {
            if (context == null)
                return;

            context.Database.EnsureCreated();

            if (context.Roles.Any() && context.Users.Any())
                return;

            Assembly assembly = Assembly.GetExecutingAssembly();

            string roleName = "Manager";
            IdentityRole role = new IdentityRole();

            if (!context.Roles.Any())
            {
                role = new IdentityRole(roleName);
                role.NormalizedName = roleName;
                context.Add(role);

                var roleUser = new IdentityRole("User");
                roleUser.NormalizedName = "User";
                context.Add(roleUser);

                await context.SaveChangesAsync();
            }

            var user = new ApplicationUser
            {
                UserName = "admin",
                NormalizedUserName = "admin",
                Email = "noreply@beetsoft.com.vn",
                NormalizedEmail = "noreply@beetsoft.com.vn",
                DisplayName = "SysAdmin",
                Address = "Hà Nội",

                IsDeleted = false,
                DateCreated = DateTime.Now,
                LastUpdated = DateTime.Now,
                CreatedBy = "Beetsoft",
                UpdatedBy = "Beetsoft",

                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user, "123321Aa@");
                user.PasswordHash = hashed;

                var userStore = new UserStore<ApplicationUser, IdentityRole, ApplicationContext>(context);
                if (userStore != null)
                {
                    await userStore.CreateAsync(user);
                    await userStore.AddToRoleAsync(user, roleName);
                }
            }

            await context.SaveChangesAsync();

        }
    }
}
