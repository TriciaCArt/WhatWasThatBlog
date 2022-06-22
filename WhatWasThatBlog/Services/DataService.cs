using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhatWasThatBlog.Data;
using WhatWasThatBlog.Models;

namespace WhatWasThatBlog.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        //will execute every time the application starts
        public async Task SetupDBAsync()
        {
            await _dbContext.Database.MigrateAsync();

            //call a private method that adds a few roles into the system
            //Adds 2 records into the ASPNetRoles table
            await SeedRolesAsync();
            await SeedUsersAsync();
            
        }



        private async Task SeedRolesAsync()
        {
            //This code makes sure nothing is done if there are ANY records in the Roles table.
            if (_roleManager.Roles.Count() == 0)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Moderator"));

            }

        }

        private async Task SeedUsersAsync()
        {
            //Create a new instance of BlogUser
            var email = "Tricia@Chitwood.com";
            
            BlogUser user = new()
            {
                UserName = "TriciaChitwood",
                Email = email,
                FirstName = "Tricia",
                LastName = "Chitwood",
                PhoneNumber = "203-426-4964",
                EmailConfirmed = true,
            };

            var adminUser = await _userManager.FindByEmailAsync(email);
            if (adminUser is null)
            {
                await _userManager.CreateAsync(user, "Abc&123!");
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            //Add someone other than you as a User and assign them to the role of moderator
            var modEmail = "Sam@clemson.edu";
            BlogUser modUser = new()
            {
                UserName = "SamGuido",
                Email = modEmail,
                FirstName = "Sam",
                LastName = "Guido",
                PhoneNumber = "203-426-5665",
                EmailConfirmed = true,

            };
            var newModUser = await _userManager.FindByEmailAsync(modEmail);
            if(newModUser is null)
            {
                await _userManager.CreateAsync(modUser, "Abc&123!");
                await _userManager.AddToRoleAsync(modUser, "Moderator");
            };

        }

        private async Task SeedTagsAsync()
        {
            await _dbContext.AddAsync(new Tag() { Text = "Bull Doo Doo" });
        }


    }
}
