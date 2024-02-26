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
    }
}
