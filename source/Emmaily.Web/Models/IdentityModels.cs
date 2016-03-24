using System.Collections.Generic;  
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Emaily.Core.Abstraction;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Emaily.Core.Data;
using Emaily.Core.Enumerations;

namespace Emaily.Web.Models
{
    public class Role : IdentityRole
    {
        public ApiAccessEnum Write { get; set; }
        public ApiAccessEnum Read { get; set; }
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public UserProfile Profile { get; set; }
        public int? ProfileId { get; set; }
        public ICollection<UserApps> Apps { get; set; } 
    }

    public class UserApps : Entity
    {
        public User User { get; set; }
        public App App { get; set; }

        [Index("UserApp", 1, IsUnique = true)]
        public string UserId { get; set; }
        [Index("UserApp", 2, IsUnique = true)]
        public int AppId { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
           //do something;
        }
    }
}