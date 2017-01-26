namespace FinalShoppingCart.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FinalShoppingCart.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FinalShoppingCart.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
 new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "AliciaMacCara@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "AliciaMacCara@gmail.com",
                    Email = "AliciaMacCara@gmail.com",
                    FirstName = "Alicia",
                    LastName = "MacCara",
                    DisplayName = "Alimariemac"
                }, "Testpassword123!");
            }

            var userId = userManager.FindByEmail("AliciaMacCara@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
        }
    }
}
