using System;
using Microsoft.Owin;
using Owin;
using practice.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartupAttribute(typeof(practice.Startup))]
namespace practice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            app.MapSignalR();
            ConfigureAuth(app);
            createRolesandUsers();
        }
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            


            // In Startup iam creating first Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                
            }

            // creating Creating Manager role     
           

            // creating Creating Employee role     
            if (!roleManager.RoleExists("Client"))
            {
                var role = new IdentityRole();
                role.Name = "Client";
                roleManager.CreateAsync(role);

            }
        }

    }
   
}
