using Jornadas_Metalurgia_2026.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jornadas_Metalurgia_2026.Config
{
    public static class ContextDbSeeder
    {
        public static async Task SeedAdminUser(ApplicationDbContext context, IConfiguration config)
        {
            var userName = config["AdminData:UserName"];
            var email = config["AdminData:Email"];
            var password = config["AdminData:Password"];





            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ){
                return;
            }

            var adminExists = await context.Users.AnyAsync(u => u.Email == email || u.UserName == userName);
            if (!adminExists)
            {
                var passwordHasher = new PasswordHasher<object>();
                var hashedPassword = passwordHasher.HashPassword(new object(), password);




                var defaultAdmin = new User
                {
                    UserName = userName,
                    Email = email,
                    Password = hashedPassword,

                };
                await context.Users.AddAsync(defaultAdmin);
                await context.SaveChangesAsync();
            }
        }
    }
}
