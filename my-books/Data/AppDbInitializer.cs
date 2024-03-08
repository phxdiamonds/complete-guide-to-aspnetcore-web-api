using Microsoft.AspNetCore.Identity;
using my_books.Data.ViewModel.Authentication;

namespace my_books.Data
{
    public class AppDbInitializer
    {
        //adding the data to the database

        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                if (!context.Books.Any())
                {
                    context.Books.AddRange(new Models.Book()
                    {
                        Title = "1st book",
                        Description = "This is 1st book",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(30),
                        Rate = 4,
                        Genre = "Biography",
                        CoverUrl = "https:.....",
                        DateAdded = DateTime.Now
                    },
                    new Models.Book()
                    {
                        Title = "2st book",
                        Description = "This is 2st book",
                        IsRead = false,
                        DateRead = DateTime.Now.AddDays(30),
                        Rate = 5,
                        Genre = "History",
                        CoverUrl = "https:.....",
                        DateAdded = DateTime.Now
                    },
                    new Models.Book()
                    {
                        Title = "3rd book",
                        Description = "This is 3rd book",
                        IsRead = true,
                        DateRead = DateTime.Now.AddDays(20),
                        Rate = 4,
                        Genre = "Phylology",
                        CoverUrl = "https:....",
                        DateAdded = DateTime.Now
                    });

                    context.SaveChanges();
                }
            }
        }

        //seed the roles to the database

        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            //Create a scope for the application services

            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                //Check the roles exist in datebase, if not add them

                if(!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if(!await roleManager.RoleExistsAsync(UserRoles.Publisher))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Publisher));
                if(!await roleManager.RoleExistsAsync(UserRoles.Author))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Author));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            }
        }
    }
}
