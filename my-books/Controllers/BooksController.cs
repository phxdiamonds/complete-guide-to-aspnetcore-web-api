using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using my_books.Data.Services;
using my_books.Data.ViewModel;
using my_books.Data.ViewModel.Authentication;

namespace my_books.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //its defines whether its an api controller or mvc controller
    public class BooksController : ControllerBase
    {
        //Here Inject the service

        public BooksServices _booksService;

        public BooksController(BooksServices booksService)
        {
            _booksService = booksService;
        }

        [HttpPost("add-book-with-authors")] //Providing custom Url
        public IActionResult AddBook([FromBody] BookVM book)
        {
            _booksService.AddBookwithAuthors(book);
            return Ok();
        }


        [Authorize(Roles = UserRoles.Author)]
        [HttpGet("get-all-books")]

        public IActionResult GetAllBooks()
        {
            var allBooks = _booksService.GetAllBooks();

            return Ok(allBooks);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBookbyId(int id)
        {
            var idBook = _booksService.GetBookById(id);
            return Ok(idBook);
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBookbyId(int id, [FromBody] BookVM book)
        {
            var updateBook = _booksService.UpdateBookById(id, book);
            return Ok(updateBook);
        }

        [HttpDelete("delete-book-by-id/{id}")]

        public IActionResult DeleteBookbyId(int id)
        {
            _booksService.DeleteBookById(id);
            return Ok();
        }
    }
}
