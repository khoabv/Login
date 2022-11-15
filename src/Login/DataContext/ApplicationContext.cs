using Login.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Login.DataContext
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);

            var tblRole = modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            tblRole.HasKey(x => x.Id).HasAnnotation("DatabaseGenerated", DatabaseGeneratedOption.Identity);
            tblRole.Property(e => e.Id).ValueGeneratedOnAdd();
            tblRole.Property(r => r.Name).IsRequired();

            //modelBuilder.Entity<IdentityRole>().ToTable("Roles").Property(r => r.Name).IsRequired();
            //modelBuilder.Entity<IdentityRole>().Property<bool>("IsSystem");

            var tblUser = modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            tblUser.Property((ApplicationUser u) => u.UserName).IsRequired();

            var tblUserRole = modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            tblUserRole.HasKey(r => new { UserId = r.UserId, RoleId = r.RoleId });

            var tblUserToken = modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
            tblUserToken.HasKey(r => new { LoginProvider = r.LoginProvider, Name = r.Name, UserId = r.UserId });

            var tblUserLogin = modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            tblUserLogin.HasKey((IdentityUserLogin<string> l) =>
                    new { UserId = l.UserId, LoginProvider = l.LoginProvider, ProviderKey = l.ProviderKey });


        }
    }
}
