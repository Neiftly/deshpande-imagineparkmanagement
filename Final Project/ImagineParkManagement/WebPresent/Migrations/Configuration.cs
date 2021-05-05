namespace WebPresent.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebPresent.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebPresent.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebPresent.Models.ApplicationDbContext";
        }

        protected override void Seed(WebPresent.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            const string admin = "admin@imaginepark.com";
            const string defaultPassword = "P@ssword1";

            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Park Manager" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Intern" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Park Staff Admin" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "QuarterMaster" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Volunteer" });
            context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Maintenance Manager" });

            context.SaveChanges();

            if(!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                    Surname = "account",
                    GivenName = "admin",
                    StartDate = DateTime.UtcNow,
                    Active = true
                };

                IdentityResult result = userManager.Create(user, defaultPassword);
                context.SaveChanges();

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }
            }
        }
    }
}
