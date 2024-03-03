using Microsoft.EntityFrameworkCore;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Exceptions;

namespace my_books_tests
{
    public class PublishersServiceTests
    {
        //InMemory Database

        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "BookDbTest").Options;

        AppDbContext context;

        PublishersService publishersService;
            
        [OneTimeSetUp] //setting database for one time and use it for all tests and later dispose it 
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated(); //Database created

            SeedDatabase();

            publishersService = new PublishersService(context);


        }

        [Test, Order(1)]//Here order is the test case execution order
        public void GetAllPublishers_withNoSortBy_withNoSerachString_withNoPageNumber()
        {
            //we will use the publisher service
            var result = publishersService.GetAllPublishers("", "", null);

            Assert.That(result.Count, Is.EqualTo(5)); //here the result.count is the number of publishers from the database and isequal to 3 we have 3 publishers

           // Assert.AreEqual(result.Count, 5);

        }

        [Test,Order(2)]
        public void GetAllPublishers_withNoSortBy_withNoSearchString_withPageNumber()
        {
            var result = publishersService.GetAllPublishers("", "", 2);

            Assert.That(result.Count, Is.EqualTo(1));

           // Assert.AreEqual(result.Count, 3);
        }

        [Test,Order(3)]
        public void GetAllPublishers_withNoSortBy_withSearchString_withNoPageNumber()
        {
            var result = publishersService.GetAllPublishers("", "C", null);

            Assert.That(result.Count, Is.EqualTo(4));
           // Assert.That(result.FirstOrDefault().Name, Is.EqualTo("crown publishing"));
        }

        [Test,Order(4)]
        public void GetAllPublishers_withSortBy_withNoSerachString_withNoPageNumber()
        {
            var result = publishersService.GetAllPublishers("name_desc", "", null);
            Assert.That(result.Count,Is.EqualTo(5));
            Assert.That(result.FirstOrDefault().Name, Is.EqualTo("Scholastic")); //Sorting with descending order
        }

        [Test, Order(5)]
        public void GetPublisherById_withValidResponse_Test()
        {
            var result = publishersService.GetPublisherById(1);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Name, Is.EqualTo("Penguin Random House"));

        }

        [Test,Order(6)]
        public void GetPublisherById_withInvalidResponse_Test()
        {
            var result = publishersService.GetPublisherById(10);
            Assert.That(result, Is.Null);

        }

        [Test, Order(7)]
        public void AddPublisher_withException_Test()
        {
            //first create the view model for the publisher

            var newPublisher = new PublisherVM() { Name = "123" };

            Assert.That(() => publishersService.AddPublisher(newPublisher), Throws.Exception.TypeOf<PublisherNameException>().With.Message.EqualTo("Publisher name start with number"));


        }

        [Test,Order(8)]
        public void AddPublisher_withValidResponse_Test()
        {
            var newPublisher = new PublisherVM() { Name = "john" };

            var result = publishersService.AddPublisher(newPublisher);

            Assert.That(result, Is.Not.Null);

            Assert.That(result.Name, Does.StartWith("j"));

            //Assert.That(result.Id,Is.Not.Null);
        }

        [Test,Order(9)]
       public void GetPublisherData_Test()
        {
            var result = publishersService.GetPublisherData(1);
            
            Assert.That(result.Name, Is.EqualTo("Penguin Random House")); //checking the publisher name

            Assert.That(result.BookAuthors, Is.Not.Empty); //checking publisher has any book author

            Assert.That(result.BookAuthors.Count, Is.GreaterThanOrEqualTo(1));  //checking count

            //getting book names and authors

            Assert.That(result.BookAuthors.OrderBy(x => x.BookName).FirstOrDefault().BookName, Is.EqualTo("Book 1"));



        }



        [OneTimeTearDown]
        public void CleanUP()
        {
            context.Database.EnsureDeleted();
            //context.Dispose();
        }

        private void SeedDatabase() //Write the test case for 
        {
            var publishers = new List<Publisher>
            {
                new Publisher () { Id = 1, Name = "Penguin Random House" },
                new Publisher () { Id = 2, Name = "HarperCollins" },
                new Publisher () { Id = 3, Name = "Macmillan Publishers" },
                new Publisher () { Id = 4, Name = "Scholastic" },
                new Publisher () { Id = 5, Name = "Crown Publishing" },
                new Publisher () { Id = 6, Name = "Bloomsbury" }
            };

            context.Publishers.AddRange(publishers);


            var authors = new List<Author>()
            {
                new Author() { Id = 1, FullName = "J. K. Rowling" },
                new Author() { Id = 2, FullName = "J. R. R. Tolkien" },
            };

            context.Authors.AddRange(authors);
            

            var books = new List<Book>()
            {
                new Book(){ Id = 1, Title = "Book 1", Description = "Book 1 Description", IsRead = false, Rate = 0, Genre = "Genre 1", CoverUrl = "https://...", DateAdded = DateTime.Now.AddDays(-10),PublisherId=1},
                new Book(){ Id = 2, Title = "Book 2", Description = "Book 2 Description", IsRead = false, Rate = 0, Genre = "Genre 2", CoverUrl = "https://...", DateAdded = DateTime.Now.AddDays(-20),PublisherId=2},
            };

            context.Books.AddRange(books);


            var books_authors = new List<Book_Author>()
            {
                new Book_Author() { id = 1, BookId = 1, AuthorId = 1 },
                new Book_Author() { id = 2, BookId = 1, AuthorId = 2 },
                new Book_Author() { id = 3, BookId = 2, AuthorId = 2},
            };

            context.Book_Authors.AddRange(books_authors);

            context.SaveChanges();
        }
    }
}