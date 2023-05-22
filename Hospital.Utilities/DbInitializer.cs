using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Utilities
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private ApplicationDbContext _context;

        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initialize()
        {
            try 
            {
                if (_context.Database.GetPendingMigrations().Count()>0) 
                {
                    _context.Database.Migrate();
                }
            }catch(Exception ex) 
            {
                throw;
            }

            if (!_roleManager.RoleExistsAsync(WebSiteRoles.WebSite_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Patient)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebSiteRoles.WebSite_Doctor)).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser 
                    { 
                         UserName = "Ahmed",
                         Email= "Ahmed30@gmail.com"
                    },
                     "Ahmed3046200"
                ).GetAwaiter().GetResult();

                var AppUser = _context.ApplicationUsers.FirstOrDefault(x=>x.Email == "Ahmed30@gmail.com");
                if (AppUser != null) 
                {
                    _userManager.AddToRoleAsync(AppUser.Id , WebSiteRoles.WebSite_Admin).GetAwaiter().GetResult();
                }
            }
        }
    }
}
