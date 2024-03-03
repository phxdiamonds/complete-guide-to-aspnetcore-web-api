using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using my_books.Controllers;
using my_books.Data;
using my_books.Data.Models;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my_books_tests
{
    public class PublisherControllerTests
    {
        //In Memory Database

        private static DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(databaseName: "BookDbControllerTest").Options;

        AppDbContext context;

        PublishersService publishersService;
        PublishersController publishersController;

        [OneTimeSetUp] //setting database for one time and use it for all tests and later dispose it 
        public void Setup()
        {
            context = new AppDbContext(dbContextOptions);
            context.Database.EnsureCreated(); //Database created

            SeedDatabase();

            publishersService = new PublishersService(context); // inject the service

            publishersController = new PublishersController(publishersService, new NullLogger<PublishersController>()); //Inject the controller, which takes 2 parameters


        }

        [Test, Order(1)]
        public void HTTPGet_GetAllPublishers_withSortBy_withSerachString_withPageNumber_ReturnOk_Test()
        {
            IActionResult actionResult = publishersController.GetAllPublishers("name_desc", "B", 1);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>()); //checking ok result

            var actionResultData = (actionResult as OkObjectResult).Value as List<Publisher>; //Getting data from action result

            //Assert.That(actionResultData.First().Name, Is.EqualTo("Bloomsbury")); //checking publisher name

           // Assert.That(actionResultData.First().Id, Is.EqualTo(6)); //checking publisher id

            Assert.That(actionResultData.Count, Is.EqualTo(3)); //checking count


            //Checking for 2nd page

            IActionResult actionResult2 = publishersController.GetAllPublishers("name_desc", "S", 2);

            Assert.That(actionResult2, Is.TypeOf<OkObjectResult>()); //checking ok result

            var actionResultData2 = (actionResult2 as OkObjectResult).Value as List<Publisher>; //Getting data from action result

           // Assert.That(actionResultData2.First().Name, Is.EqualTo("Scholastic")); //checking publisher name

            //Assert.That(actionResultData2.First().Id, Is.EqualTo(4)); //checking publisher id

            Assert.That(actionResultData2.Count, Is.EqualTo(1)); //checking count





        }

        [Test, Order(2)]
        public void HTTPGet_GetPublisherById_ReturnOk_Test()
        {
            IActionResult actionResult = publishersController.GetPublisherById(1);

            Assert.That(actionResult, Is.TypeOf<OkObjectResult>()); //checking ok result

            var actionResultData = (actionResult as OkObjectResult).Value as Publisher; //Getting data from action result, because we have and objectresult

            Assert.That(actionResultData.Name, Is.EqualTo("Penguin Random House").IgnoreCase); //checking publisher name

            Assert.That(actionResultData.Id, Is.EqualTo(1)); //checking publisher id

        }

        [Test, Order(3)]
        public void HTTPGet_GetPublisherById_NotFound_Test()
        {
            IActionResult actionResult = publishersController.GetPublisherById(99);

            Assert.That(actionResult, Is.TypeOf<NotFoundResult>()); //checking ok result

            

        }

        [Test,Order(4)]
        public void HTTPPost_AddPublisher_ReturnsCreated_Test()
        {
            var newPublisherVM = new PublisherVM() { Name = "john" };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<CreatedResult>());


        }

        [Test, Order(5)]
        public void HTTPPost_AddPublisher_ReturnsBadRequest_Test()
        {
            var newPublisherVM = new PublisherVM() { Name = "123" };

            IActionResult actionResult = publishersController.AddPublisher(newPublisherVM);

            Assert.That(actionResult, Is.TypeOf<BadRequestObjectResult>()); //because we are passing some data in bad request, so we used badrequestobjectresult


        }

        [Test, Order(6)]

        public void HTTPDelete_DeletePublisherById_ReturnsOk_Test()
        {
            IActionResult deletere = publishersController.DeletePublisherById(1);

            Assert.That(deletere, Is.TypeOf<OkResult>());
        }

        [Test, Order(7)]
        public void HTTPDelete_DeletePublisherById_BadRequest_Test()
        {
            IActionResult deletere = publishersController.DeletePublisherById(99);

            Assert.That(deletere, Is.TypeOf<BadRequestObjectResult>());
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

        [OneTimeTearDown]
        public void CleanUP()
        {
            context.Database.EnsureDeleted();
            //context.Dispose();
        }
    }
}
