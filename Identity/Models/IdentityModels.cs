using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class BrahmanSecurityUser : IdentityUser<int, BrahmanSecurityUserLogin, BrahmanSecurityUserRole, BrahmanSecurityUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<BrahmanSecurityUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        
    }



    public class BrahmanSecurityDbContext : IdentityDbContext<BrahmanSecurityUser, BrahmanSecurityRole, int, BrahmanSecurityUserLogin, BrahmanSecurityUserRole, BrahmanSecurityUserClaim>
    {
        public BrahmanSecurityDbContext()
            : base("BrahmanSecurityConnection")
        {
        }

        public static BrahmanSecurityDbContext Create()
        {
            return new BrahmanSecurityDbContext();
        }

        /// <summary>
        /// This is used to give control over the database structure used 
        /// by the default implementation of OnModelCreating.
        /// </summary>
        /// <param name="modelBuilder">CLR to database mapper</param>
        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            // Run the base model creating
            base.OnModelCreating(modelBuilder);

            // Update the model now that it's been created
            modelBuilder.Entity<BrahmanSecurityUser>().ToTable("Users").Property(p => p.PhoneNumber).HasMaxLength(64);
            modelBuilder.Entity<BrahmanSecurityUser>().ToTable("Users").Property(p => p.UserName).HasMaxLength(256);
            modelBuilder.Entity<BrahmanSecurityUser>().ToTable("Users").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<BrahmanSecurityRole>().ToTable("Roles").Property(p => p.Id).HasColumnName("RoleId");
            modelBuilder.Entity<BrahmanSecurityUserClaim>().ToTable("UserClaims").Property(p => p.Id).HasColumnName("UserClaimId");
            modelBuilder.Entity<BrahmanSecurityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<BrahmanSecurityUserLogin>().ToTable("UserLogins");
        }
    }


    /// <summary>
    /// Extension class for the purpose of implementing 
    /// database integer type keys
    /// </summary>
    public class BrahmanSecurityRole : IdentityRole<int, BrahmanSecurityUserRole>
    {
    }

    /// <summary>
    /// Extension class for the purpose of implementing 
    /// database integer type keys
    /// </summary>
    public class BrahmanSecurityUserRole : IdentityUserRole<int>
    {
    }

    /// <summary>
    /// Extension class for the purpose of implementing 
    /// database integer type keys
    /// </summary>
    public class BrahmanSecurityUserLogin : IdentityUserLogin<int>
    {
    }

    /// <summary>
    /// Extension class for the purpose of implementing 
    /// database integer type keys
    /// </summary>
    public class BrahmanSecurityUserClaim : IdentityUserClaim<int>
    {
    }
}