using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oyooni.Server.Constants;
using Oyooni.Server.Data.BusinessModels;
using System;

namespace Oyooni.Server.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="ModelBuilder"/> class
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Seeds initial data
        /// </summary>
        public static void SeedData(this ModelBuilder builder)
        {
            // Create the admin role
            var adminRole = new IdentityRole
            {
                Id = Guid.NewGuid().ToString(),
                Name = Roles.Admin,
                NormalizedName = Roles.Admin.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            // Create the admin user 
            var adminUser = new AppUser
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Admin",
                UserName = "admin@oyooni.com",
                NormalizedUserName = "ADMIN@OYOONI.COM",
                NormalizedEmail = "ADMIN@OYOONI.COM",
                Email = "admin@oyooni.com",
                EmailConfirmed = true
            };

            // Add the hashed password
            adminUser.PasswordHash = new PasswordHasher<AppUser>().HashPassword(adminUser, "Admin.123");

            // Add the role
            builder.Entity<IdentityRole>().HasData(adminRole);

            // Add the user
            builder.Entity<AppUser>().HasData(adminUser);

            // Add the UserRole entry
            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUser.Id,
                RoleId = adminRole.Id
            });
        }
    }
}
