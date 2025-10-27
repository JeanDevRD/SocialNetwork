using Microsoft.AspNetCore.Identity;
using SocialNetwork.Infrastructure.Identity.Entities;


namespace SocialNetwork.Infrastructure.Identity.Seed
{
    public static class DefaultUser
    {
        public static async Task SeedAsync(UserManager<UserEntity> userManager)
        {

            if (await userManager.FindByEmailAsync("Jean@Gmail.com") == null)
            {
                var user = new UserEntity
                {
                    UserName = "Jean",
                    Email = "Jean@Gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Jean",
                    LastName = "Carlos",
                    PhoneNumber = "1234567890",
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    Profile = "Images/default_profile.png"
                };
                if (userManager.Users.All(u => u.Id != user.Id))
                {
                    await userManager.CreateAsync(user,"Jean123!");
                }
            }
            if (await userManager.FindByEmailAsync("Jean2@Gmail.com") == null)
            {
                var user = new UserEntity
                {
                    UserName = "Jean2",
                    Email = "Jean2@Gmail.com",
                    EmailConfirmed = true,
                    FirstName = "Jean",
                    LastName = "Carlos",
                    PhoneNumber = "1234567891",
                    PhoneNumberConfirmed = true,
                    IsActive = true,
                    Profile = "Images/default_profile.png"
                };
                if (userManager.Users.All(u => u.Id != user.Id))
                {
                    await userManager.CreateAsync(user, "Jean123!");
                }
            }
        }

    }
}
